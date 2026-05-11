using MediatR;

namespace eCommerce.Features.Products.DeactivateProduct
{
    public record DeactivateProductCommand(Guid Id) : IRequest;
}
