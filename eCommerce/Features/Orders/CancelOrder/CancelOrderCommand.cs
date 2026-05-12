using MediatR;

namespace eCommerce.Features.Orders.CancelOrder
{
    public record CancelOrderCommand(Guid Id) : IRequest;
}
