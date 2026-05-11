using MediatR;

namespace eCommerce.Features.Cart.RemoveCartItem
{
    public record RemoveCartItemCommand(Guid ProductId) : IRequest;
}
