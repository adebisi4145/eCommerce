using eCommerce.Domain.Entities;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Categories.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<CreateCategoryHandler> _logger;

        public CreateCategoryHandler(ECommerceDbContext dbContext, ILogger<CreateCategoryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating category {CategoryName}", request.Name);

            var nameExists = await _dbContext.Categories
                .AnyAsync(c => c.Name == request.Name, cancellationToken);

            if (nameExists)
                throw new InvalidOperationException($"Category '{request.Name}' already exists");

            var category = new Category(request.Name);

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category {CategoryId} created: {CategoryName}", category.Id, category.Name);

            return new CreateCategoryResponse(category.Id);
        }
    }
}
