using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Categories.GetCategories
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, GetCategoriesResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<GetCategoriesHandler> _logger;

        public GetCategoriesHandler(ECommerceDbContext dbContext, ILogger<GetCategoriesHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<GetCategoriesResponse> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all categories");

            var categories = await _dbContext.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new CategorySummary(c.Id, c.Name))
                .ToListAsync(cancellationToken);

            return new GetCategoriesResponse(categories);
        }
    }
}
