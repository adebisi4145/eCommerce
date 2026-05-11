using MediatR;

namespace eCommerce.Features.Products.UpdateStock
{
    public record UpdateStockCommand(Guid Id, int Stock) : IRequest;
}
