using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Categories.GetCategory
{
    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, GetCategoryResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<GetCategoryHandler> _logger;

        public GetCategoryHandler(ECommerceDbContext dbContext, ILogger<GetCategoryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<GetCategoryResponse> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving category {CategoryId}", request.Id);

            var category = await _dbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
            {
                _logger.LogWarning("Category {CategoryId} not found", request.Id);
                throw new KeyNotFoundException("Category not found");
            }

            return new GetCategoryResponse(category.Id, category.Name);
        }
    }
}
