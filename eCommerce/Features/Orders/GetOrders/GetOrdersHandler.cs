using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Orders.GetOrders
{
    public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, GetOrdersResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<GetOrdersHandler> _logger;

        public GetOrdersHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<GetOrdersHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<GetOrdersResponse> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Retrieving orders for user {UserId}", userId);

            var orders = await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderSummary(
                    o.Id,
                    o.Status.ToString(),
                    o.CreatedAt,
                    o.TotalAmount,
                    o.Items.Count))
                .ToListAsync(cancellationToken);

            return new GetOrdersResponse(orders);
        }
    }
}
