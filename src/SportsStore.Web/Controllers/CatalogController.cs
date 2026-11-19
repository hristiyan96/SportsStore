using Microsoft.AspNetCore.Mvc;
using SportsStore.Services.Catalog;

namespace SportsStore.Web.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalog;

    public CatalogController(ICatalogService catalog)
    {
        _catalog = catalog;
    }

    public async Task<IActionResult> Index(string? search, int? categoryId, int page = 1, CancellationToken cancellationToken = default)
    {
        var model = await _catalog.GetPageAsync(search, categoryId, page, cancellationToken);
        return View(model);
    }
}
