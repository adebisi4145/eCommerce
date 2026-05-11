using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Products.UpdateStock
{
    public class UpdateStockHandler : IRequestHandler<UpdateStockCommand>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<UpdateStockHandler> _logger;

        public UpdateStockHandler(ECommerceDbContext dbContext, ILogger<UpdateStockHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating stock for product {ProductId} to {Stock}", request.Id, request.Stock);

            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning("Product {ProductId} not found", request.Id);
                throw new KeyNotFoundException("Product not found");
            }

            product.SetStock(request.Stock);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Stock for product {ProductId} updated to {Stock}", request.Id, request.Stock);
        }
    }
}
