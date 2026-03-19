using eCommerce.Domain.Entities;
using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eCommerce.Features.Users.AddAddress
{
    public class AddAddressHandler : IRequestHandler<AddAddressCommand, AddAddressResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<AddAddressHandler> _logger;

        public AddAddressHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<AddAddressHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<AddAddressResponse> Handle(AddAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Adding address for user {UserId}", userId);

            var user = await _dbContext.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found when adding address", userId);
                throw new KeyNotFoundException("User not found");
            }

            var address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode, request.IsDefault);

            user.AddAddress(address);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Address {AddressId} added for user {UserId}", address.Id, userId);

            return new AddAddressResponse(address.Id);
        }
    }
}
