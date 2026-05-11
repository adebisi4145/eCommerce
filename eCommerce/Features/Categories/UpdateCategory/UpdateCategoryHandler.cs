using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Categories.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<UpdateCategoryHandler> _logger;

        public UpdateCategoryHandler(ECommerceDbContext dbContext, ILogger<UpdateCategoryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating category {CategoryId}", request.Id);

            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
            {
                _logger.LogWarning("Category {CategoryId} not found", request.Id);
                throw new KeyNotFoundException("Category not found");
            }

            var nameExists = await _dbContext.Categories
                .AnyAsync(c => c.Name == request.Name && c.Id != request.Id, cancellationToken);

            if (nameExists)
                throw new InvalidOperationException($"Category '{request.Name}' already exists");

            category.UpdateName(request.Name);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category {CategoryId} updated to {CategoryName}", request.Id, request.Name);
        }
    }
}
