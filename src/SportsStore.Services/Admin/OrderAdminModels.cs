using SportsStore.Core.Enums;

namespace SportsStore.Services.Admin;

public class AdminOrderRow
{
    public int Id { get; init; }

    public string CustomerName { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public decimal Total { get; init; }

    public OrderStatus Status { get; init; }

    public DateTime CreatedOn { get; init; }

    public int ItemCount { get; init; }
}

public class AdminOrderDetail
{
    public int Id { get; init; }

    public string CustomerName { get; init; } = string.Empty;

    public string Phone { get; init; } = string.Empty;

    public string ShippingAddress { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public decimal Total { get; init; }

    public OrderStatus Status { get; init; }

    public DateTime CreatedOn { get; init; }

    public IReadOnlyList<AdminOrderLine> Lines { get; init; } = Array.Empty<AdminOrderLine>();
}

public class AdminOrderLine
{
    public string ProductName { get; init; } = string.Empty;

    public int Quantity { get; init; }

    public decimal UnitPrice { get; init; }
}
