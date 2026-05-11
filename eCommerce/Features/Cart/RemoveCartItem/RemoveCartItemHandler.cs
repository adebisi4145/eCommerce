using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Cart.RemoveCartItem
{
    public class RemoveCartItemHandler : IRequestHandler<RemoveCartItemCommand>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<RemoveCartItemHandler> _logger;

        public RemoveCartItemHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<RemoveCartItemHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Removing product {ProductId} from cart for user {UserId}", request.ProductId, userId);

            var cart = await _dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null)
                throw new KeyNotFoundException("Cart not found");

            cart.RemoveItem(request.ProductId);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product {ProductId} removed from cart for user {UserId}", request.ProductId, userId);
        }
    }
}
