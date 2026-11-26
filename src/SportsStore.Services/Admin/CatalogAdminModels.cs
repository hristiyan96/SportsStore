namespace SportsStore.Services.Admin;

public class CatalogEntryRow
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Detail { get; init; } = string.Empty;

    public bool IsActive { get; init; }

    public int ProductCount { get; init; }
}

public class CategoryInput
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public class BrandInput
{
    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
