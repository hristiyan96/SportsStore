using Microsoft.EntityFrameworkCore;
using SportsStore.Data;

namespace SportsStore.Services.Catalog;

public class CatalogService : ICatalogService
{
    private readonly ApplicationDbContext _db;

    public CatalogService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<CatalogPage> GetPageAsync(string? search, int? categoryId, int page, CancellationToken cancellationToken = default)
    {
        var term = string.IsNullOrWhiteSpace(search) ? null : search.Trim();
        var requestedPage = page < 1 ? 1 : page;

        var categories = await _db.Categories
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => new CategoryOption { Id = c.Id, Name = c.Name })
            .ToListAsync(cancellationToken);

        var selectedCategory = categoryId is int id && categories.Any(c => c.Id == id) ? id : (int?)null;

        var products = _db.Products
            .AsNoTracking()
            .Where(p => p.IsActive);

        if (selectedCategory is int category)
        {
            products = products.Where(p => p.CategoryId == category);
        }

        if (term is not null)
        {
            products = products.Where(p =>
                p.Name.Contains(term) ||
                p.Sku.Contains(term) ||
                p.Description.Contains(term) ||
                p.Brand.Name.Contains(term));
        }

        var totalCount = await products.CountAsync(cancellationToken);
        var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)CatalogPage.PageSize);
        var currentPage = requestedPage > totalPages ? totalPages : requestedPage;

        var items = await products
            .OrderBy(p => p.Name)
            .Skip((currentPage - 1) * CatalogPage.PageSize)
            .Take(CatalogPage.PageSize)
            .Select(p => new CatalogItem
            {
                Id = p.Id,
                Name = p.Name,
                Sku = p.Sku,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name
            })
            .ToListAsync(cancellationToken);

        return new CatalogPage
        {
            Items = items,
            Categories = categories,
            Search = term,
            CategoryId = selectedCategory,
            Page = currentPage,
            TotalCount = totalCount
        };
    }

    public async Task<ProductDetails?> GetDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Products
            .AsNoTracking()
            .Where(p => p.Id == id && p.IsActive)
            .Select(p => new ProductDetails
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Sku = p.Sku,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
                BrandCountry = p.Brand.Country
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
