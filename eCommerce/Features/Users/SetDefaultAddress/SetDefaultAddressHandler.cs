using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Users.SetDefaultAddress
{
    public class SetDefaultAddressHandler : IRequestHandler<SetDefaultAddressCommand>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<SetDefaultAddressHandler> _logger;

        public SetDefaultAddressHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<SetDefaultAddressHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Setting default address {AddressId} for user {UserId}", request.AddressId, userId);

            var user = await _dbContext.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found when setting default address {AddressId}", userId, request.AddressId);
                throw new KeyNotFoundException("User not found");
            }

            user.SetDefaultAddress(request.AddressId);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Address {AddressId} set as default for user {UserId}", request.AddressId, userId);
        }
    }
}
