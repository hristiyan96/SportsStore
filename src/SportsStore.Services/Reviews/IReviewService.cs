namespace SportsStore.Services.Reviews;

public interface IReviewService
{
    Task<IReadOnlyList<ProductReviewItem>> ListAsync(int productId, CancellationToken cancellationToken = default);

    Task<bool> HasReviewedAsync(int productId, string userId, CancellationToken cancellationToken = default);

    Task<string?> AddAsync(int productId, string userId, int rating, string comment, CancellationToken cancellationToken = default);
}
