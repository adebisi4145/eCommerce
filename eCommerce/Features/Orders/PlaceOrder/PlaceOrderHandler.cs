using eCommerce.Domain.Entities;
using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Orders.PlaceOrder
{
    public class PlaceOrderHandler : IRequestHandler<PlaceOrderCommand, PlaceOrderResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<PlaceOrderHandler> _logger;

        public PlaceOrderHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<PlaceOrderHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<PlaceOrderResponse> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Placing order for user {UserId}", userId);

            var user = await _dbContext.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            var shippingAddress = user.Addresses.FirstOrDefault(a => a.Id == request.ShippingAddressId);
            if (shippingAddress == null)
                throw new KeyNotFoundException("Shipping address not found");

            var cart = await _dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null || !cart.Items.Any())
                throw new InvalidOperationException("Cart is empty");

            var productIds = cart.Items.Select(i => i.ProductId).ToList();

            var products = await _dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, cancellationToken);

            foreach (var item in cart.Items)
            {
                if (!products.TryGetValue(item.ProductId, out var product))
                    throw new InvalidOperationException($"Product {item.ProductId} not found");

                if (!product.IsActive)
                    throw new InvalidOperationException($"Product '{product.Name}' is no longer available");

                if (product.Stock < item.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for '{product.Name}'. Only {product.Stock} unit(s) available");
            }

            var order = new Order(
                userId,
                shippingAddress.Street,
                shippingAddress.City,
                shippingAddress.State,
                shippingAddress.Country,
                shippingAddress.ZipCode);

            foreach (var item in cart.Items)
            {
                var product = products[item.ProductId];
                order.AddItem(product.Id, product.Name, product.Price, item.Quantity);
                product.SetStock(product.Stock - item.Quantity);
            }

            _dbContext.Orders.Add(order);
            cart.Clear();

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Order {OrderId} placed for user {UserId}", order.Id, userId);

            return new PlaceOrderResponse(order.Id);
        }
    }
}
