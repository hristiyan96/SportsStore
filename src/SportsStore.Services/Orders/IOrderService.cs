namespace SportsStore.Services.Orders;

public interface IOrderService
{
    Task<PlacedOrder> PlaceAsync(IReadOnlyList<Cart.StoredCartLine> lines, CheckoutDetails details, string? userId = null, CancellationToken cancellationToken = default);

    Task<OrderReceipt?> GetReceiptAsync(int orderId, CancellationToken cancellationToken = default);
}
