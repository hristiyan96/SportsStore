using Microsoft.EntityFrameworkCore;
using SportsStore.Core.Enums;
using SportsStore.Data;

namespace SportsStore.Services.Admin;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _db;

    public DashboardService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<StoreDashboard> GetAsync(CancellationToken cancellationToken = default)
    {
        var orders = _db.Orders.AsNoTracking();
        var products = _db.Products.AsNoTracking().Where(product => product.IsActive);

        return new StoreDashboard
        {
            OrderCount = await orders.CountAsync(cancellationToken),
            PendingOrderCount = await orders.CountAsync(order => order.Status == OrderStatus.Pending, cancellationToken),
            OrderTotal = await orders.SumAsync(order => (decimal?)order.Total, cancellationToken) ?? 0m,
            ProductCount = await products.CountAsync(cancellationToken),
            UnitsInStock = await products.SumAsync(product => (int?)product.StockQuantity, cancellationToken) ?? 0,
            OutOfStockCount = await products.CountAsync(product => product.StockQuantity == 0, cancellationToken)
        };
    }
}
