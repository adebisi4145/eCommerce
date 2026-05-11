using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Categories.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<DeleteCategoryHandler> _logger;

        public DeleteCategoryHandler(ECommerceDbContext dbContext, ILogger<DeleteCategoryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting category {CategoryId}", request.Id);

            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
            {
                _logger.LogWarning("Category {CategoryId} not found", request.Id);
                throw new KeyNotFoundException("Category not found");
            }

            var hasProducts = await _dbContext.Products
                .AnyAsync(p => p.CategoryId == request.Id, cancellationToken);

            if (hasProducts)
                throw new InvalidOperationException("Cannot delete a category that has products assigned to it. Reassign or deactivate all products in this category first.");

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category {CategoryId} deleted", request.Id);
        }
    }
}
