using MediatR;

namespace eCommerce.Features.Categories.GetCategories
{
    public record GetCategoriesQuery : IRequest<GetCategoriesResponse>;
}
