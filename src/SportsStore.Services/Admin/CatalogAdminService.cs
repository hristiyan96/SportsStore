using Microsoft.EntityFrameworkCore;
using SportsStore.Core.Entities;
using SportsStore.Data;

namespace SportsStore.Services.Admin;

public class CatalogAdminService : ICatalogAdminService
{
    private readonly ApplicationDbContext _db;

    public CatalogAdminService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<CatalogEntryRow>> ListCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .Select(category => new CatalogEntryRow
            {
                Id = category.Id,
                Name = category.Name,
                Detail = category.Description,
                IsActive = category.IsActive,
                ProductCount = category.Products.Count
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryInput?> GetCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Categories
            .AsNoTracking()
            .Where(category => category.Id == id)
            .Select(category => new CategoryInput
            {
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string?> CreateCategoryAsync(CategoryInput input, CancellationToken cancellationToken = default)
    {
        var name = input.Name.Trim();
        if (await _db.Categories.AnyAsync(category => category.Name == name, cancellationToken))
        {
            return "That name is already used.";
        }

        _db.Categories.Add(new Category
        {
            Name = name,
            Description = input.Description.Trim(),
            IsActive = input.IsActive,
            CreatedOn = DateTime.UtcNow
        });
        await _db.SaveChangesAsync(cancellationToken);
        return null;
    }

    public async Task<string?> UpdateCategoryAsync(int id, CategoryInput input, CancellationToken cancellationToken = default)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (category is null)
        {
            return "That category was not found.";
        }

        var name = input.Name.Trim();
        if (await _db.Categories.AnyAsync(item => item.Name == name && item.Id != id, cancellationToken))
        {
            return "That name is already used.";
        }

        category.Name = name;
        category.Description = input.Description.Trim();
        category.IsActive = input.IsActive;
        await _db.SaveChangesAsync(cancellationToken);
        return null;
    }

    public Task DeactivateCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        return _db.Categories
            .Where(category => category.Id == id && category.IsActive)
            .ExecuteUpdateAsync(setters => setters.SetProperty(category => category.IsActive, false), cancellationToken);
    }

    public async Task<IReadOnlyList<CatalogEntryRow>> ListBrandsAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Brands
            .AsNoTracking()
            .OrderBy(brand => brand.Name)
            .Select(brand => new CatalogEntryRow
            {
                Id = brand.Id,
                Name = brand.Name,
                Detail = brand.Country,
                IsActive = brand.IsActive,
                ProductCount = brand.Products.Count
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<BrandInput?> GetBrandAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Brands
            .AsNoTracking()
            .Where(brand => brand.Id == id)
            .Select(brand => new BrandInput
            {
                Name = brand.Name,
                Country = brand.Country,
                IsActive = brand.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string?> CreateBrandAsync(BrandInput input, CancellationToken cancellationToken = default)
    {
        var name = input.Name.Trim();
        if (await _db.Brands.AnyAsync(brand => brand.Name == name, cancellationToken))
        {
            return "That name is already used.";
        }

        _db.Brands.Add(new Brand
        {
            Name = name,
            Country = input.Country.Trim(),
            IsActive = input.IsActive,
            CreatedOn = DateTime.UtcNow
        });
        await _db.SaveChangesAsync(cancellationToken);
        return null;
    }

    public async Task<string?> UpdateBrandAsync(int id, BrandInput input, CancellationToken cancellationToken = default)
    {
        var brand = await _db.Brands.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (brand is null)
        {
            return "That brand was not found.";
        }

        var name = input.Name.Trim();
        if (await _db.Brands.AnyAsync(item => item.Name == name && item.Id != id, cancellationToken))
        {
            return "That name is already used.";
        }

        brand.Name = name;
        brand.Country = input.Country.Trim();
        brand.IsActive = input.IsActive;
        await _db.SaveChangesAsync(cancellationToken);
        return null;
    }

    public Task DeactivateBrandAsync(int id, CancellationToken cancellationToken = default)
    {
        return _db.Brands
            .Where(brand => brand.Id == id && brand.IsActive)
            .ExecuteUpdateAsync(setters => setters.SetProperty(brand => brand.IsActive, false), cancellationToken);
    }

}
