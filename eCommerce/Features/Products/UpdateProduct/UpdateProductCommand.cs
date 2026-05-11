using MediatR;

namespace eCommerce.Features.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price) : IRequest;
}
