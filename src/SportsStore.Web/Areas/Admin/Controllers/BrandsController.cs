using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Data.Seeding;
using SportsStore.Services.Admin;
using SportsStore.Web.Models;

namespace SportsStore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = IdentitySeeder.AdministratorRole)]
public class BrandsController : Controller
{
    private readonly ICatalogAdminService _catalog;

    public BrandsController(ICatalogAdminService catalog)
    {
        _catalog = catalog;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(await _catalog.ListBrandsAsync(cancellationToken));
    }

    public IActionResult Create()
    {
        return View(new BrandForm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BrandForm form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var error = await _catalog.CreateBrandAsync(form.ToInput(), cancellationToken);
        if (error is not null)
        {
            ModelState.AddModelError(string.Empty, error);
            return View(form);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var brand = await _catalog.GetBrandAsync(id, cancellationToken);
        return brand is null ? NotFound() : View(BrandForm.From(brand));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BrandForm form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var error = await _catalog.UpdateBrandAsync(id, form.ToInput(), cancellationToken);
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
        await _catalog.DeactivateBrandAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
