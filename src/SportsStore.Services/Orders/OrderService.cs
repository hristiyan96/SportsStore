using Microsoft.EntityFrameworkCore;
using SportsStore.Core.Entities;
using SportsStore.Core.Enums;
using SportsStore.Data;
using SportsStore.Services.Cart;

namespace SportsStore.Services.Orders;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _db;

    public OrderService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PlacedOrder> PlaceAsync(IReadOnlyList<StoredCartLine> lines, CheckoutDetails details, CancellationToken cancellationToken = default)
    {
        var name = details.CustomerName.Trim();
        var phone = details.Phone.Trim();
        var address = details.ShippingAddress.Trim();
        var city = details.City.Trim();

        if (name.Length is < 2 or > 120 || phone.Length is < 5 or > 40 || address.Length is < 5 or > 240 || city.Length is < 2 or > 80)
        {
            return new PlacedOrder { Error = "Check the name, phone, address, and city." };
        }

        var requested = lines
            .Where(line => line.Quantity > 0)
            .GroupBy(line => line.ProductId)
            .Select(group => new StoredCartLine { ProductId = group.Key, Quantity = group.Sum(line => line.Quantity) })
            .ToList();

        if (requested.Count == 0)
        {
            return new PlacedOrder { Error = "The cart is empty." };
        }

        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        var ids = requested.Select(line => line.ProductId).ToArray();
        var products = await _db.Products
            .AsNoTracking()
            .Where(product => ids.Contains(product.Id) && product.IsActive)
            .ToListAsync(cancellationToken);

        if (products.Count != requested.Count)
        {
            return new PlacedOrder { Error = "A product in the cart is no longer available." };
        }

        foreach (var line in requested)
        {
            var updated = await _db.Products
                .Where(product => product.Id == line.ProductId && product.IsActive && product.StockQuantity >= line.Quantity)
                .ExecuteUpdateAsync(setters => setters.SetProperty(product => product.StockQuantity, product => product.StockQuantity - line.Quantity), cancellationToken);

            if (updated != 1)
            {
                await transaction.RollbackAsync(cancellationToken);
                var product = products.First(item => item.Id == line.ProductId);
                return new PlacedOrder { Error = $"Only {product.StockQuantity} of {product.Name} are in stock." };
            }
        }

        var order = new CustomerOrder
        {
            CustomerName = name,
            Phone = phone,
            ShippingAddress = address,
            City = city,
            Status = OrderStatus.Pending,
            CreatedOn = DateTime.UtcNow,
            Items = requested.Select(line =>
            {
                var product = products.First(item => item.Id == line.ProductId);
                return new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = line.Quantity,
                    UnitPrice = product.Price,
                    CreatedOn = DateTime.UtcNow
                };
            }).ToList()
        };

        order.Total = order.Items.Sum(item => item.Quantity * item.UnitPrice);
        _db.Orders.Add(order);
        await _db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return new PlacedOrder { OrderId = order.Id, Total = order.Total };
    }

    public async Task<OrderReceipt?> GetReceiptAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return await _db.Orders
            .AsNoTracking()
            .Where(order => order.Id == orderId)
            .Select(order => new OrderReceipt
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                Phone = order.Phone,
                ShippingAddress = order.ShippingAddress,
                City = order.City,
                Total = order.Total,
                Status = order.Status.ToString(),
                Lines = order.Items
                    .OrderBy(item => item.ProductName)
                    .Select(item => new OrderReceiptLine
                    {
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
