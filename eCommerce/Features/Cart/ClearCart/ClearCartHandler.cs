using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Cart.ClearCart
{
    public class ClearCartHandler : IRequestHandler<ClearCartCommand>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<ClearCartHandler> _logger;

        public ClearCartHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<ClearCartHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Clearing cart for user {UserId}", userId);

            var cart = await _dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null)
                throw new KeyNotFoundException("Cart not found");

            cart.Clear();

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Cart cleared for user {UserId}", userId);
        }
    }
}
