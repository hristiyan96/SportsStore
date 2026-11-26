using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Data.Seeding;
using SportsStore.Services.Admin;
using SportsStore.Web.Models;

namespace SportsStore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = IdentitySeeder.AdministratorRole)]
public class CategoriesController : Controller
{
    private readonly ICatalogAdminService _catalog;

    public CategoriesController(ICatalogAdminService catalog)
    {
        _catalog = catalog;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(await _catalog.ListCategoriesAsync(cancellationToken));
    }

    public IActionResult Create()
    {
        return View(new CategoryForm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryForm form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var error = await _catalog.CreateCategoryAsync(form.ToInput(), cancellationToken);
        if (error is not null)
        {
            ModelState.AddModelError(string.Empty, error);
            return View(form);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var category = await _catalog.GetCategoryAsync(id, cancellationToken);
        return category is null ? NotFound() : View(CategoryForm.From(category));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryForm form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var error = await _catalog.UpdateCategoryAsync(id, form.ToInput(), cancellationToken);
        if (error is not null)
        {
            ModelState.AddModelError(string.Empty, error);
            return View(form);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken)
    {
        await _catalog.DeactivateCategoryAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
