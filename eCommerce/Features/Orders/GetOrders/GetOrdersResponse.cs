namespace eCommerce.Features.Orders.GetOrders
{
    public record OrderSummary(Guid Id, string Status, DateTime CreatedAt, decimal TotalAmount, int ItemCount);

    public record GetOrdersResponse(IEnumerable<OrderSummary> Orders);
}
