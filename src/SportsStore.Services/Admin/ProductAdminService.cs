using Microsoft.EntityFrameworkCore;
using SportsStore.Core.Entities;
using SportsStore.Data;

namespace SportsStore.Services.Admin;

public class ProductAdminService : IProductAdminService
{
    private readonly ApplicationDbContext _db;

    public ProductAdminService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<AdminProductRow>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Products
            .AsNoTracking()
            .OrderBy(product => product.Name)
            .Select(product => new AdminProductRow
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductSave?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Products
            .AsNoTracking()
            .Where(product => product.Id == id)
            .Select(product => new ProductSave
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
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<IReadOnlyList<CatalogChoice>> CategoriesAsync(CancellationToken cancellationToken = default)
    {
        return Choices(_db.Categories.Where(item => item.IsActive).Select(item => new CatalogChoice { Id = item.Id, Name = item.Name }), cancellationToken);
    }

    public Task<IReadOnlyList<CatalogChoice>> BrandsAsync(CancellationToken cancellationToken = default)
    {
        return Choices(_db.Brands.Where(item => item.IsActive).Select(item => new CatalogChoice { Id = item.Id, Name = item.Name }), cancellationToken);
    }

    public Task<IReadOnlyList<CatalogChoice>> SuppliersAsync(CancellationToken cancellationToken = default)
    {
        return Choices(_db.Suppliers.Where(item => item.IsActive).Select(item => new CatalogChoice { Id = item.Id, Name = item.Name }), cancellationToken);
    }

    public async Task<string?> CreateAsync(ProductSave input, CancellationToken cancellationToken = default)
    {
        var error = await ValidateAsync(input, null, cancellationToken);
        if (error is not null)
        {
            return error;
        }

        _db.Products.Add(new Product
        {
            Name = input.Name.Trim(),
            Description = input.Description.Trim(),
            Sku = input.Sku.Trim(),
            Price = input.Price,
            StockQuantity = input.StockQuantity,
            CategoryId = input.CategoryId,
            BrandId = input.BrandId,
            SupplierId = input.SupplierId,
            IsActive = input.IsActive,
            CreatedOn = DateTime.UtcNow
        });

        await _db.SaveChangesAsync(cancellationToken);
        return null;
    }

    public async Task<string?> UpdateAsync(int id, ProductSave input, CancellationToken cancellationToken = default)
    {
        var product = await _db.Products.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (product is null)
        {
            return "That product was not found.";
        }

        var error = await ValidateAsync(input, id, cancellationToken);
        if (error is not null)
        {
            return error;
        }

        product.Name = input.Name.Trim();
        product.Description = input.Description.Trim();
        product.Sku = input.Sku.Trim();
        product.Price = input.Price;
        product.StockQuantity = input.StockQuantity;
        product.CategoryId = input.CategoryId;
        product.BrandId = input.BrandId;
        product.SupplierId = input.SupplierId;
        product.IsActive = input.IsActive;
        await _db.SaveChangesAsync(cancellationToken);
        return null;
    }

    public async Task<bool> DeactivateAsync(int id, CancellationToken cancellationToken = default)
    {
        var updated = await _db.Products
            .Where(product => product.Id == id && product.IsActive)
            .ExecuteUpdateAsync(setters => setters.SetProperty(product => product.IsActive, false), cancellationToken);

        return updated == 1;
    }

    private async Task<string?> ValidateAsync(ProductSave input, int? currentId, CancellationToken cancellationToken)
    {
        if (input.Price < 0 || input.StockQuantity < 0)
        {
            return "Price and stock cannot be negative.";
        }

        var referencesExist = await _db.Categories.AnyAsync(item => item.Id == input.CategoryId && item.IsActive, cancellationToken)
            && await _db.Brands.AnyAsync(item => item.Id == input.BrandId && item.IsActive, cancellationToken)
            && await _db.Suppliers.AnyAsync(item => item.Id == input.SupplierId && item.IsActive, cancellationToken);

        if (!referencesExist)
        {
            return "Choose a category, brand, and supplier.";
        }

        var sku = input.Sku.Trim();
        var skuTaken = await _db.Products.AnyAsync(product => product.Sku == sku && product.Id != currentId, cancellationToken);
        return skuTaken ? "That SKU is already used." : null;
    }

    private static async Task<IReadOnlyList<CatalogChoice>> Choices(IQueryable<CatalogChoice> query, CancellationToken cancellationToken)
    {
        return await query.OrderBy(item => item.Name).ToListAsync(cancellationToken);
    }
}
