namespace SportsStore.Services.Orders;

public interface IOrderService
{
    Task<PlacedOrder> PlaceAsync(IReadOnlyList<Cart.StoredCartLine> lines, CheckoutDetails details, CancellationToken cancellationToken = default);

    Task<OrderReceipt?> GetReceiptAsync(int orderId, CancellationToken cancellationToken = default);
}
