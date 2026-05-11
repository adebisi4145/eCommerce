using MediatR;

namespace eCommerce.Features.Categories.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id) : IRequest;
}
