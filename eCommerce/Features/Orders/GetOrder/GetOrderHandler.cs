using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Orders.GetOrder
{
    public class GetOrderHandler : IRequestHandler<GetOrderQuery, GetOrderResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<GetOrderHandler> _logger;

        public GetOrderHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<GetOrderHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<GetOrderResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Retrieving order {OrderId} for user {UserId}", request.Id, userId);

            var order = await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order == null)
                throw new KeyNotFoundException("Order not found");

            if (order.UserId != userId)
                throw new KeyNotFoundException("Order not found");

            var items = order.Items.Select(i => new OrderItemDto(
                i.ProductId,
                i.ProductName,
                i.UnitPrice,
                i.Quantity,
                i.Subtotal));

            var shippingAddress = new ShippingAddressDto(
                order.ShippingStreet,
                order.ShippingCity,
                order.ShippingState,
                order.ShippingCountry,
                order.ShippingZipCode);

            return new GetOrderResponse(
                order.Id,
                order.Status.ToString(),
                order.CreatedAt,
                order.TotalAmount,
                shippingAddress,
                items);
        }
    }
}
