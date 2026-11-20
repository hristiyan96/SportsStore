namespace SportsStore.Services.Catalog;

public interface ICatalogService
{
    Task<CatalogPage> GetPageAsync(string? search, int? categoryId, int page, CancellationToken cancellationToken = default);

    Task<ProductDetails?> GetDetailsAsync(int id, CancellationToken cancellationToken = default);
}
