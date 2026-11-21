namespace SportsStore.Services.Cart;

public interface ICartService
{
    Task<ShoppingCart> GetAsync(IReadOnlyList<StoredCartLine> lines, CancellationToken cancellationToken = default);

    Task<ShoppingCart> AddAsync(IReadOnlyList<StoredCartLine> lines, int productId, int quantity, CancellationToken cancellationToken = default);

    Task<ShoppingCart> SetQuantityAsync(IReadOnlyList<StoredCartLine> lines, int productId, int quantity, CancellationToken cancellationToken = default);

    ShoppingCart Remove(IReadOnlyList<StoredCartLine> lines, int productId);
}
