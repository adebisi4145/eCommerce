using MediatR;

namespace eCommerce.Features.Categories.CreateCategory
{
    public record CreateCategoryCommand(string Name) : IRequest<CreateCategoryResponse>;
}
