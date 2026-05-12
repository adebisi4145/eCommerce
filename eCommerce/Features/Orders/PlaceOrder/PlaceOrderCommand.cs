using MediatR;

namespace eCommerce.Features.Orders.PlaceOrder
{
    public record PlaceOrderCommand(Guid ShippingAddressId) : IRequest<PlaceOrderResponse>;
}
