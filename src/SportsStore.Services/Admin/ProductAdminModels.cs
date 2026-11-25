namespace SportsStore.Services.Admin;

public class AdminProductRow
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Sku { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public int StockQuantity { get; init; }

    public bool IsActive { get; init; }

    public string CategoryName { get; init; } = string.Empty;

    public string BrandName { get; init; } = string.Empty;
}

public class ProductSave
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Sku { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public int CategoryId { get; set; }

    public int BrandId { get; set; }

    public int SupplierId { get; set; }

    public bool IsActive { get; set; } = true;
}

public class CatalogChoice
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;
}
