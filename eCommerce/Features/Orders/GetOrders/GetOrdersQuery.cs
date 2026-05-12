using MediatR;

namespace eCommerce.Features.Orders.GetOrders
{
    public record GetOrdersQuery : IRequest<GetOrdersResponse>;
}
