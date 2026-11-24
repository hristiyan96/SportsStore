namespace SportsStore.Services.Admin;

public interface IDashboardService
{
    Task<StoreDashboard> GetAsync(CancellationToken cancellationToken = default);
}
