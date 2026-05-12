using eCommerce.Domain.Enums;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Orders.UpdateOrderStatus
{
    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<UpdateOrderStatusHandler> _logger;

        public UpdateOrderStatusHandler(ECommerceDbContext dbContext, ILogger<UpdateOrderStatusHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating status of order {OrderId} to {Status}", request.Id, request.Status);

            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order == null)
                throw new KeyNotFoundException("Order not found");

            var status = Enum.Parse<OrderStatus>(request.Status, ignoreCase: true);
            order.UpdateStatus(status);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Order {OrderId} status updated to {Status}", request.Id, request.Status);
        }
    }
}
