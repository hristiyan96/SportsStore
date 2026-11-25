namespace SportsStore.Services.Admin;

public interface IProductAdminService
{
    Task<IReadOnlyList<AdminProductRow>> ListAsync(CancellationToken cancellationToken = default);

    Task<ProductSave?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CatalogChoice>> CategoriesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CatalogChoice>> BrandsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CatalogChoice>> SuppliersAsync(CancellationToken cancellationToken = default);

    Task<string?> CreateAsync(ProductSave input, CancellationToken cancellationToken = default);

    Task<string?> UpdateAsync(int id, ProductSave input, CancellationToken cancellationToken = default);

    Task<bool> DeactivateAsync(int id, CancellationToken cancellationToken = default);
}
