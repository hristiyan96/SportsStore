using Microsoft.EntityFrameworkCore;
using SportsStore.Data;

namespace SportsStore.Services.Cart;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _db;

    public CartService(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<ShoppingCart> GetAsync(IReadOnlyList<StoredCartLine> lines, CancellationToken cancellationToken = default)
    {
        return BuildAsync(Normalize(lines), null, cancellationToken);
    }

    public async Task<ShoppingCart> AddAsync(IReadOnlyList<StoredCartLine> lines, int productId, int quantity, CancellationToken cancellationToken = default)
    {
        if (quantity < 1)
        {
            return await BuildAsync(Normalize(lines), "Enter a quantity of at least 1.", cancellationToken);
        }

        var stock = await StockForAsync(productId, cancellationToken);
        if (stock is null)
        {
            return await BuildAsync(Normalize(lines), "That product is no longer available.", cancellationToken);
        }

        var current = Normalize(lines);
        var existing = current.FirstOrDefault(line => line.ProductId == productId)?.Quantity ?? 0;
        if (existing + quantity > stock.Value)
        {
            return await BuildAsync(current, $"Only {stock.Value} are in stock.", cancellationToken);
        }

        var updated = current
            .Where(line => line.ProductId != productId)
            .Append(new StoredCartLine { ProductId = productId, Quantity = existing + quantity })
            .ToList();

        return await BuildAsync(updated, null, cancellationToken);
    }

    public async Task<ShoppingCart> SetQuantityAsync(IReadOnlyList<StoredCartLine> lines, int productId, int quantity, CancellationToken cancellationToken = default)
    {
        var current = Normalize(lines);
        if (quantity < 1)
        {
            return await BuildAsync(current, "Enter a quantity of at least 1.", cancellationToken);
        }

        if (current.All(line => line.ProductId != productId))
        {
            return await BuildAsync(current, "That product is not in the cart.", cancellationToken);
        }

        var stock = await StockForAsync(productId, cancellationToken);
        if (stock is null)
        {
            return await BuildAsync(current.Where(line => line.ProductId != productId).ToList(), "That product is no longer available.", cancellationToken);
        }

        if (quantity > stock.Value)
        {
            return await BuildAsync(current, $"Only {stock.Value} are in stock.", cancellationToken);
        }

        var updated = current
            .Select(line => line.ProductId == productId
                ? new StoredCartLine { ProductId = productId, Quantity = quantity }
                : line)
            .ToList();

        return await BuildAsync(updated, null, cancellationToken);
    }

    public ShoppingCart Remove(IReadOnlyList<StoredCartLine> lines, int productId)
    {
        var updated = Normalize(lines).Where(line => line.ProductId != productId).ToList();
        return new ShoppingCart { Lines = updated };
    }

    private async Task<ShoppingCart> BuildAsync(IReadOnlyList<StoredCartLine> lines, string? error, CancellationToken cancellationToken)
    {
        if (lines.Count == 0)
        {
            return new ShoppingCart { Error = error, Lines = lines };
        }

        var ids = lines.Select(line => line.ProductId).ToArray();
        var products = await _db.Products
            .AsNoTracking()
            .Where(product => ids.Contains(product.Id) && product.IsActive)
            .Select(product => new
            {
                product.Id,
                product.Name,
                product.Sku,
                product.Price,
                product.StockQuantity
            })
            .ToListAsync(cancellationToken);

        var byId = products.ToDictionary(product => product.Id);
        var kept = new List<StoredCartLine>();
        var items = new List<CartItem>();

        foreach (var line in lines)
        {
            if (!byId.TryGetValue(line.ProductId, out var product))
            {
                continue;
            }

            var quantity = Math.Min(line.Quantity, product.StockQuantity);
            if (quantity < 1)
            {
                continue;
            }

            kept.Add(new StoredCartLine { ProductId = line.ProductId, Quantity = quantity });
            items.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                UnitPrice = product.Price,
                Quantity = quantity,
                StockQuantity = product.StockQuantity
            });
        }

        return new ShoppingCart
        {
            Items = items,
            Lines = kept,
            Error = error
        };
    }

    private async Task<int?> StockForAsync(int productId, CancellationToken cancellationToken)
    {
        return await _db.Products
            .AsNoTracking()
            .Where(product => product.Id == productId && product.IsActive)
            .Select(product => (int?)product.StockQuantity)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private static List<StoredCartLine> Normalize(IReadOnlyList<StoredCartLine> lines)
    {
        return lines
            .Where(line => line.Quantity > 0)
            .GroupBy(line => line.ProductId)
            .Select(group => new StoredCartLine
            {
                ProductId = group.Key,
                Quantity = group.Sum(line => line.Quantity)
            })
            .ToList();
    }
}
