using Microsoft.AspNetCore.Mvc;
using SportsStore.Services.Cart;
using SportsStore.Web.Infrastructure;

namespace SportsStore.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cart;

    public CartController(ICartService cart)
    {
        _cart = cart;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var current = await _cart.GetAsync(CartSession.Read(HttpContext.Session), cancellationToken);
        CartSession.Write(HttpContext.Session, current.Lines);

        if (TempData["CartError"] is string error)
        {
            current = new ShoppingCart
            {
                Items = current.Items,
                Lines = current.Lines,
                Error = error
            };
        }

        return View(current);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity, CancellationToken cancellationToken)
    {
        var result = await _cart.AddAsync(CartSession.Read(HttpContext.Session), productId, quantity, cancellationToken);
        CartSession.Write(HttpContext.Session, result.Lines);

        if (result.Error is not null)
        {
            TempData["CartError"] = result.Error;
            return RedirectToAction("Details", "Catalog", new { id = productId });
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int productId, int quantity, CancellationToken cancellationToken)
    {
        var result = await _cart.SetQuantityAsync(CartSession.Read(HttpContext.Session), productId, quantity, cancellationToken);
        CartSession.Write(HttpContext.Session, result.Lines);

        if (result.Error is not null)
        {
            TempData["CartError"] = result.Error;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int productId)
    {
        var result = _cart.Remove(CartSession.Read(HttpContext.Session), productId);
        CartSession.Write(HttpContext.Session, result.Lines);
        return RedirectToAction(nameof(Index));
    }
}
