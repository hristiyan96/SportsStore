using Microsoft.EntityFrameworkCore;
using SportsStore.Core.Enums;
using SportsStore.Data;

namespace SportsStore.Services.Admin;

public class OrderAdminService : IOrderAdminService
{
    private readonly ApplicationDbContext _db;

    public OrderAdminService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<AdminOrderRow>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Orders
            .AsNoTracking()
            .OrderByDescending(order => order.CreatedOn)
            .Select(order => new AdminOrderRow
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                City = order.City,
                Total = order.Total,
                Status = order.Status,
                CreatedOn = order.CreatedOn,
                ItemCount = order.Items.Count
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<AdminOrderDetail?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.Orders
            .AsNoTracking()
            .Where(order => order.Id == id)
            .Select(order => new AdminOrderDetail
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                Phone = order.Phone,
                ShippingAddress = order.ShippingAddress,
                City = order.City,
                Total = order.Total,
                Status = order.Status,
                CreatedOn = order.CreatedOn,
                Lines = order.Items
                    .OrderBy(item => item.ProductName)
                    .Select(item => new AdminOrderLine
                    {
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> UpdateStatusAsync(int id, OrderStatus status, CancellationToken cancellationToken = default)
    {
        var updated = await _db.Orders
            .Where(order => order.Id == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(order => order.Status, status), cancellationToken);

        return updated == 1;
    }
}
