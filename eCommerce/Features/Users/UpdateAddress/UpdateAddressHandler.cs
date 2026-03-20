using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Users.UpdateAddress
{
    public class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<UpdateAddressHandler> _logger;

        public UpdateAddressHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<UpdateAddressHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Updating address {AddressId} for user {UserId}", request.AddressId, userId);

            var user = await _dbContext.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found when updating address {AddressId}", userId, request.AddressId);
                throw new KeyNotFoundException("User not found");
            }

            user.UpdateAddress(request.AddressId, request.Street, request.City, request.State, request.Country, request.ZipCode);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Address {AddressId} updated for user {UserId}", request.AddressId, userId);
        }
    }
}