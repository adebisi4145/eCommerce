using MediatR;

namespace eCommerce.Features.Cart.GetCart
{
    public record GetCartQuery : IRequest<GetCartResponse>;
}
