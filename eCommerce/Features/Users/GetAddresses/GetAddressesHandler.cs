using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Users.GetAddresses
{
    public class GetAddressesHandler : IRequestHandler<GetAddressesQuery, List<GetAddressesResponse>>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<GetAddressesHandler> _logger;

        public GetAddressesHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<GetAddressesHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<List<GetAddressesResponse>> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Retrieving addresses for user {UserId}", userId);

            var addresses = await _dbContext.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Addresses)
                .Select(a => new GetAddressesResponse(a.Id, a.Street, a.City, a.State, a.Country, a.ZipCode, a.IsDefault))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} addresses for user {UserId}", addresses.Count, userId);

            return addresses;
        }
    }
}
