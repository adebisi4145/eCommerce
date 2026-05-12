using MediatR;

namespace eCommerce.Features.Orders.GetOrder
{
    public record GetOrderQuery(Guid Id) : IRequest<GetOrderResponse>;
}
