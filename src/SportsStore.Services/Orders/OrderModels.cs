namespace SportsStore.Services.Orders;

public class CheckoutDetails
{
    public string CustomerName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string ShippingAddress { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;
}

public class PlacedOrder
{
    public int? OrderId { get; init; }

    public decimal Total { get; init; }

    public string? Error { get; init; }
}

public class OrderReceipt
{
    public int Id { get; init; }

    public string CustomerName { get; init; } = string.Empty;

    public string Phone { get; init; } = string.Empty;

    public string ShippingAddress { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public decimal Total { get; init; }

    public string Status { get; init; } = string.Empty;

    public IReadOnlyList<OrderReceiptLine> Lines { get; init; } = Array.Empty<OrderReceiptLine>();
}

public class OrderReceiptLine
{
    public string ProductName { get; init; } = string.Empty;

    public int Quantity { get; init; }

    public decimal UnitPrice { get; init; }

    public decimal LineTotal => UnitPrice * Quantity;
}
