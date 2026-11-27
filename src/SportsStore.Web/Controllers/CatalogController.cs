using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Services.Catalog;
using SportsStore.Services.Reviews;
using SportsStore.Web.Models;

namespace SportsStore.Web.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalog;
    private readonly IReviewService _reviews;

    public CatalogController(ICatalogService catalog, IReviewService reviews)
    {
        _catalog = catalog;
        _reviews = reviews;
    }

    public async Task<IActionResult> Index(string? search, int? categoryId, int page = 1, CancellationToken cancellationToken = default)
    {
        var model = await _catalog.GetPageAsync(search, categoryId, page, cancellationToken);
        return View(model);
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken = default)
    {
        var product = await _catalog.GetDetailsAsync(id, cancellationToken);
        if (product is null)
        {
            return NotFound();
        }

        return View(await PageAsync(product, cancellationToken));
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Review(int id, ProductPage form, CancellationToken cancellationToken)
    {
        var product = await _catalog.GetDetailsAsync(id, cancellationToken);
        if (product is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            var invalid = await PageAsync(product, cancellationToken);
            invalid.Rating = form.Rating;
            invalid.Comment = form.Comment;
            return View("Details", invalid);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var error = await _reviews.AddAsync(id, userId, form.Rating, form.Comment, cancellationToken);
        if (error is not null)
        {
            ModelState.AddModelError(string.Empty, error);
            var rejected = await PageAsync(product, cancellationToken);
            rejected.Rating = form.Rating;
            rejected.Comment = form.Comment;
            return View("Details", rejected);
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    private async Task<ProductPage> PageAsync(SportsStore.Services.Catalog.ProductDetails product, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var alreadyReviewed = userId is not null && await _reviews.HasReviewedAsync(product.Id, userId, cancellationToken);

        return new ProductPage
        {
            Product = product,
            Reviews = await _reviews.ListAsync(product.Id, cancellationToken),
            CanReview = userId is not null,
            AlreadyReviewed = alreadyReviewed
        };
    }
}
