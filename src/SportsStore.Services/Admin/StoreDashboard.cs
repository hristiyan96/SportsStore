namespace SportsStore.Services.Admin;

public class StoreDashboard
{
    public int OrderCount { get; init; }

    public int PendingOrderCount { get; init; }

    public decimal OrderTotal { get; init; }

    public int ProductCount { get; init; }

    public int UnitsInStock { get; init; }

    public int OutOfStockCount { get; init; }
}
