using MediatR;

namespace eCommerce.Features.Cart.UpdateCartItem
{
    public record UpdateCartItemCommand(Guid ProductId, int Quantity) : IRequest;
}
