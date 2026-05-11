using MediatR;

namespace eCommerce.Features.Categories.UpdateCategory
{
    public record UpdateCategoryCommand(Guid Id, string Name) : IRequest;
}
