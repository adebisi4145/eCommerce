using MediatR;

namespace eCommerce.Features.Cart.AddCartItem
{
    public record AddCartItemCommand(Guid ProductId, int Quantity) : IRequest;
}
