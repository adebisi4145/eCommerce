using MediatR;

namespace eCommerce.Features.Products.GetProduct
{
    public record GetProductQuery(Guid Id) : IRequest<GetProductResponse>;
}
