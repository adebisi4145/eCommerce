using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Users.RemoveAddress
{
    public class RemoveAddressHandler : IRequestHandler<RemoveAddressCommand>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<RemoveAddressHandler> _logger;

        public RemoveAddressHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<RemoveAddressHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task Handle(RemoveAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Removing address {AddressId} for user {UserId}", request.AddressId, userId);

            var user = await _dbContext.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found when removing address {AddressId}", userId, request.AddressId);
                throw new KeyNotFoundException("User not found");
            }

            user.RemoveAddress(request.AddressId);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Address {AddressId} removed for user {UserId}", request.AddressId, userId);
        }
    }

}
