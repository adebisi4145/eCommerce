using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Products.DeactivateProduct
{
    public class DeactivateProductHandler : IRequestHandler<DeactivateProductCommand>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<DeactivateProductHandler> _logger;

        public DeactivateProductHandler(ECommerceDbContext dbContext, ILogger<DeactivateProductHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deactivating product {ProductId}", request.Id);

            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning("Product {ProductId} not found", request.Id);
                throw new KeyNotFoundException("Product not found");
            }

            product.Deactivate();

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product {ProductId} deactivated", request.Id);
        }
    }
}
