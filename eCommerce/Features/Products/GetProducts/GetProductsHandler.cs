using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Products.GetProducts
{
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, GetProductsResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<GetProductsHandler> _logger;

        public GetProductsHandler(ECommerceDbContext dbContext, ILogger<GetProductsHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<GetProductsResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving products (page {Page}, pageSize {PageSize}, categoryId {CategoryId})",
                request.Page, request.PageSize, request.CategoryId);

            var query = _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            if (request.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == request.CategoryId.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var products = await query
                .OrderBy(p => p.Name)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProductSummary(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.Stock,
                    p.IsActive,
                    p.CategoryId,
                    p.Category.Name))
                .ToListAsync(cancellationToken);

            return new GetProductsResponse(products, totalCount, request.Page, request.PageSize);
        }
    }
}
