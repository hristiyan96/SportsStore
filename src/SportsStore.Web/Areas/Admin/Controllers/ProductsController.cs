using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Data.Seeding;
using SportsStore.Services.Admin;
using SportsStore.Web.Models;

namespace SportsStore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = IdentitySeeder.AdministratorRole)]
public class ProductsController : Controller
{
    private readonly IProductAdminService _products;

    public ProductsController(IProductAdminService products)
    {
        _products = products;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var products = await _products.ListAsync(cancellationToken);
        return View(products);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await LoadChoicesAsync(cancellationToken);
        return View(new ProductForm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductForm form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadChoicesAsync(cancellationToken);
            return View(form);
        }

        var error = await _products.CreateAsync(form.ToSave(), cancellationToken);
        if (error is not null)
        {
            ModelState.AddModelError(string.Empty, error);
            await LoadChoicesAsync(cancellationToken);
            return View(form);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var product = await _products.GetAsync(id, cancellationToken);
        if (product is null)
        {
            return NotFound();
        }

        await LoadChoicesAsync(cancellationToken);
        return View(ProductForm.From(product));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductForm form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadChoicesAsync(cancellationToken);
            return View(form);
        }

        var error = await _products.UpdateAsync(id, form.ToSave(), cancellationToken);
        if (error is not null)
        {
            ModelState.AddModelError(string.Empty, error);
            await LoadChoicesAsync(cancellationToken);
            return View(form);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken)
    {
        await _products.DeactivateAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadChoicesAsync(CancellationToken cancellationToken)
    {
        ViewData["Categories"] = await _products.CategoriesAsync(cancellationToken);
        ViewData["Brands"] = await _products.BrandsAsync(cancellationToken);
        ViewData["Suppliers"] = await _products.SuppliersAsync(cancellationToken);
    }
}
