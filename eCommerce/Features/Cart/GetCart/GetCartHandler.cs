using eCommerce.Domain.Entities;
using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Cart.GetCart
{
    public class GetCartHandler : IRequestHandler<GetCartQuery, GetCartResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<GetCartHandler> _logger;

        public GetCartHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<GetCartHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<GetCartResponse> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Retrieving cart for user {UserId}", userId);

            var cart = await _dbContext.Carts
                .AsNoTracking()
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null)
                return new GetCartResponse(Guid.Empty, [], 0);

            var productIds = cart.Items.Select(i => i.ProductId).ToList();

            var products = await _dbContext.Products
                .AsNoTracking()
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, cancellationToken);

            var items = cart.Items.Select(i =>
            {
                var product = products[i.ProductId];
                var subtotal = product.Price * i.Quantity;
                return new CartItemDto(i.ProductId, product.Name, product.Price, i.Quantity, subtotal);
            }).ToList();

            var total = items.Sum(i => i.Subtotal);

            return new GetCartResponse(cart.Id, items, total);
        }
    }
}
