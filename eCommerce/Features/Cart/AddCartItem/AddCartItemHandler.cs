using eCommerce.Domain.Entities;
using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Cart.AddCartItem
{
    public class AddCartItemHandler : IRequestHandler<AddCartItemCommand>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<AddCartItemHandler> _logger;

        public AddCartItemHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<AddCartItemHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Adding product {ProductId} to cart for user {UserId}", request.ProductId, userId);

            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            if (!product.IsActive)
                throw new InvalidOperationException("Product is no longer available");

            if (product.Stock < request.Quantity)
                throw new InvalidOperationException($"Insufficient stock. Only {product.Stock} unit(s) available");

            var cart = await _dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null)
            {
                cart = new Cart(userId);
                _dbContext.Carts.Add(cart);
            }

            cart.AddItem(request.ProductId, request.Quantity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product {ProductId} added to cart for user {UserId}", request.ProductId, userId);
        }
    }
}
