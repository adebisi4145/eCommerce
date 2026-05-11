using eCommerce.Domain.Entities;
using eCommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace eCommerce.Features.Products.CreateProduct
{
    public class CreateProductHandler: IRequestHandler<CreateProductCommand, CreateProductResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<CreateProductHandler> _logger;

        public CreateProductHandler(ECommerceDbContext dbContext, ILogger<CreateProductHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating product {ProductName} in category {CategoryId}", request.Name, request.CategoryId);

            var categoryExists = await _dbContext.Categories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                _logger.LogWarning("Attempted to create product with invalid category {CategoryId}", request.CategoryId);
                throw new InvalidOperationException("Invalid category");
            }

            var product = new Product(
                request.Name,
                request.Description,
                request.Price,
                request.Stock,
                request.CategoryId
            );

            _dbContext.Products.Add(product);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product {ProductId} created: {ProductName}", product.Id, product.Name);

            return new CreateProductResponse(product.Id);
        }
    }
}
