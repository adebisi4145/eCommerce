using MediatR;

namespace eCommerce.Features.Categories.GetCategory
{
    public record GetCategoryQuery(Guid Id) : IRequest<GetCategoryResponse>;
}
