using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Core.Enums;
using SportsStore.Data.Seeding;
using SportsStore.Services.Admin;

namespace SportsStore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = IdentitySeeder.AdministratorRole)]
public class OrdersController : Controller
{
    private readonly IOrderAdminService _orders;

    public OrdersController(IOrderAdminService orders)
    {
        _orders = orders;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var orders = await _orders.ListAsync(cancellationToken);
        return View(orders);
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var order = await _orders.GetAsync(id, cancellationToken);
        return order is null ? NotFound() : View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, OrderStatus status, string? returnTo, CancellationToken cancellationToken)
    {
        var updated = await _orders.UpdateStatusAsync(id, status, cancellationToken);
        if (!updated)
        {
            return NotFound();
        }

        if (returnTo == "details")
        {
            return RedirectToAction(nameof(Details), new { id });
        }

        return RedirectToAction(nameof(Index));
    }
}
