using MediatR;

namespace eCommerce.Features.Orders.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand(Guid Id, string Status) : IRequest;
}
