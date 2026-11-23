using Microsoft.AspNetCore.Mvc;
using SportsStore.Services.Cart;
using SportsStore.Services.Orders;
using SportsStore.Web.Infrastructure;
using SportsStore.Web.Models;

namespace SportsStore.Web.Controllers;

public class CheckoutController : Controller
{
    private readonly ICartService _cart;
    private readonly IOrderService _orders;

    public CheckoutController(ICartService cart, IOrderService orders)
    {
        _cart = cart;
        _orders = orders;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var current = await LoadCartAsync(cancellationToken);
        if (current.Items.Count == 0)
        {
            return RedirectToAction("Index", "Cart");
        }

        ViewData["Cart"] = current;
        return View(new CheckoutForm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CheckoutForm form, CancellationToken cancellationToken)
    {
        var current = await LoadCartAsync(cancellationToken);
        if (current.Items.Count == 0)
        {
            return RedirectToAction("Index", "Cart");
        }

        if (!ModelState.IsValid)
        {
            ViewData["Cart"] = current;
            return View(form);
        }

        var placed = await _orders.PlaceAsync(current.Lines, new CheckoutDetails
        {
            CustomerName = form.CustomerName,
            Phone = form.Phone,
            ShippingAddress = form.ShippingAddress,
            City = form.City
        }, User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, cancellationToken);

        if (placed.Error is not null || placed.OrderId is null)
        {
            ModelState.AddModelError(string.Empty, placed.Error ?? "The order could not be placed.");
            current = await LoadCartAsync(cancellationToken);
            ViewData["Cart"] = current;
            return View(form);
        }

        CartSession.Write(HttpContext.Session, Array.Empty<StoredCartLine>());
        return RedirectToAction(nameof(Confirmation), new { id = placed.OrderId });
    }

    public async Task<IActionResult> Confirmation(int id, CancellationToken cancellationToken)
    {
        var receipt = await _orders.GetReceiptAsync(id, cancellationToken);
        if (receipt is null)
        {
            return NotFound();
        }

        return View(receipt);
    }

    private async Task<ShoppingCart> LoadCartAsync(CancellationToken cancellationToken)
    {
        var current = await _cart.GetAsync(CartSession.Read(HttpContext.Session), cancellationToken);
        CartSession.Write(HttpContext.Session, current.Lines);
        return current;
    }
}
