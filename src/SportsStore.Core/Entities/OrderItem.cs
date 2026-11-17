namespace SportsStore.Core.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }

    public CustomerOrder Order { get; set; } = null!;

    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal => Quantity * UnitPrice;
}
