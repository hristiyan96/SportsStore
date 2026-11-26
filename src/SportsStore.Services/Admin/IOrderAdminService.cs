using SportsStore.Core.Enums;

namespace SportsStore.Services.Admin;

public interface IOrderAdminService
{
    Task<IReadOnlyList<AdminOrderRow>> ListAsync(CancellationToken cancellationToken = default);

    Task<AdminOrderDetail?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> UpdateStatusAsync(int id, OrderStatus status, CancellationToken cancellationToken = default);
}
