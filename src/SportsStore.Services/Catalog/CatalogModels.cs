namespace SportsStore.Services.Catalog;

public class CatalogItem
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Sku { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public int StockQuantity { get; init; }

    public string CategoryName { get; init; } = string.Empty;

    public string BrandName { get; init; } = string.Empty;
}

public class ProductDetails
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Sku { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public int StockQuantity { get; init; }

    public string CategoryName { get; init; } = string.Empty;

    public string BrandName { get; init; } = string.Empty;

    public string BrandCountry { get; init; } = string.Empty;
}

public class CategoryOption
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;
}

public class CatalogPage
{
    public const int PageSize = 6;

    public IReadOnlyList<CatalogItem> Items { get; init; } = Array.Empty<CatalogItem>();

    public IReadOnlyList<CategoryOption> Categories { get; init; } = Array.Empty<CategoryOption>();

    public string? Search { get; init; }

    public int? CategoryId { get; init; }

    public int Page { get; init; } = 1;

    public int TotalCount { get; init; }

    public int TotalPages => TotalCount == 0 ? 1 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}
