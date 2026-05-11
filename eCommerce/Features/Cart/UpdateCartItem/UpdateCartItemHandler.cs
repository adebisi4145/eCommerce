using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Cart.UpdateCartItem
{
    public class UpdateCartItemHandler : IRequestHandler<UpdateCartItemCommand>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<UpdateCartItemHandler> _logger;

        public UpdateCartItemHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<UpdateCartItemHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Updating quantity of product {ProductId} in cart for user {UserId}", request.ProductId, userId);

            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            if (product.Stock < request.Quantity)
                throw new InvalidOperationException($"Insufficient stock. Only {product.Stock} unit(s) available");

            var cart = await _dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null)
                throw new KeyNotFoundException("Cart not found");

            cart.UpdateItemQuantity(request.ProductId, request.Quantity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Updated quantity of product {ProductId} to {Quantity} for user {UserId}", request.ProductId, request.Quantity, userId);
        }
    }
}
