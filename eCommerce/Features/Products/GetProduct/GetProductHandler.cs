using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Products.GetProduct
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, GetProductResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<GetProductHandler> _logger;

        public GetProductHandler(ECommerceDbContext dbContext, ILogger<GetProductHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<GetProductResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving product {ProductId}", request.Id);

            var product = await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning("Product {ProductId} not found", request.Id);
                throw new KeyNotFoundException("Product not found");
            }

            return new GetProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.IsActive,
                product.CategoryId,
                product.Category.Name);
        }
    }
}
