using SportsStore.Core.Enums;

namespace SportsStore.Core.Entities;

public class CustomerOrder : BaseEntity
{
    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public string CustomerName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string ShippingAddress { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
