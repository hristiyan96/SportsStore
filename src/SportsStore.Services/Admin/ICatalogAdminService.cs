namespace SportsStore.Services.Admin;

public interface ICatalogAdminService
{
    Task<IReadOnlyList<CatalogEntryRow>> ListCategoriesAsync(CancellationToken cancellationToken = default);

    Task<CategoryInput?> GetCategoryAsync(int id, CancellationToken cancellationToken = default);

    Task<string?> CreateCategoryAsync(CategoryInput input, CancellationToken cancellationToken = default);

    Task<string?> UpdateCategoryAsync(int id, CategoryInput input, CancellationToken cancellationToken = default);

    Task DeactivateCategoryAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CatalogEntryRow>> ListBrandsAsync(CancellationToken cancellationToken = default);

    Task<BrandInput?> GetBrandAsync(int id, CancellationToken cancellationToken = default);

    Task<string?> CreateBrandAsync(BrandInput input, CancellationToken cancellationToken = default);

    Task<string?> UpdateBrandAsync(int id, BrandInput input, CancellationToken cancellationToken = default);

    Task DeactivateBrandAsync(int id, CancellationToken cancellationToken = default);
}
