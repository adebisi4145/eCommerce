using MediatR;

namespace eCommerce.Features.Products.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, decimal Price, int Stock, Guid CategoryId) : IRequest<CreateProductResponse>;
}
