using MediatR;

namespace eCommerce.Features.Products.GetProducts
{
    public record GetProductsQuery(Guid? CategoryId, int Page, int PageSize) : IRequest<GetProductsResponse>;
}
