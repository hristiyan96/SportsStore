namespace SportsStore.Core.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Sku { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public int BrandId { get; set; }

    public Brand Brand { get; set; } = null!;

    public int SupplierId { get; set; }

    public Supplier Supplier { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
}
