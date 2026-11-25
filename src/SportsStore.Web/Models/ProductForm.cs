using System.ComponentModel.DataAnnotations;
using SportsStore.Services.Admin;

namespace SportsStore.Web.Models;

public class ProductForm
{
    [Required]
    [StringLength(140)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(40)]
    public string Sku { get; set; } = string.Empty;

    [Range(0, 100000)]
    public decimal Price { get; set; }

    [Range(0, 100000)]
    [Display(Name = "Stock")]
    public int StockQuantity { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Display(Name = "Brand")]
    public int BrandId { get; set; }

    [Display(Name = "Supplier")]
    public int SupplierId { get; set; }

    [Display(Name = "On sale")]
    public bool IsActive { get; set; } = true;

    public ProductSave ToSave()
    {
        return new ProductSave
        {
            Name = Name,
            Description = Description,
            Sku = Sku,
            Price = Price,
            StockQuantity = StockQuantity,
            CategoryId = CategoryId,
            BrandId = BrandId,
            SupplierId = SupplierId,
            IsActive = IsActive
        };
    }

    public static ProductForm From(ProductSave product)
    {
        return new ProductForm
        {
            Name = product.Name,
            Description = product.Description,
            Sku = product.Sku,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId,
            SupplierId = product.SupplierId,
            IsActive = product.IsActive
        };
    }
}
