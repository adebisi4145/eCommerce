using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Products.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<UpdateProductHandler> _logger;

        public UpdateProductHandler(ECommerceDbContext dbContext, ILogger<UpdateProductHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating product {ProductId}", request.Id);

            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning("Product {ProductId} not found", request.Id);
                throw new KeyNotFoundException("Product not found");
            }

            product.UpdateDetails(request.Name, request.Description);
            product.UpdatePrice(request.Price);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product {ProductId} updated", request.Id);
        }
    }
}
