namespace SportsStore.Services.Cart;

public class StoredCartLine
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }
}

public class CartItem
{
    public int ProductId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Sku { get; init; } = string.Empty;

    public decimal UnitPrice { get; init; }

    public int Quantity { get; init; }

    public int StockQuantity { get; init; }

    public decimal LineTotal => UnitPrice * Quantity;
}

public class ShoppingCart
{
    public IReadOnlyList<CartItem> Items { get; init; } = Array.Empty<CartItem>();

    public IReadOnlyList<StoredCartLine> Lines { get; init; } = Array.Empty<StoredCartLine>();

    public string? Error { get; init; }

    public decimal Total => Items.Sum(item => item.LineTotal);

    public int ItemCount => Items.Sum(item => item.Quantity);
}
