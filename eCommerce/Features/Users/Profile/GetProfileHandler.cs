using eCommerce.Infrastructure.Auth;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Users.Profile
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery, GetProfileResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<GetProfileHandler> _logger;

        public GetProfileHandler(ECommerceDbContext dbContext, ICurrentUserService currentUser, ILogger<GetProfileHandler> logger)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<GetProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();

            _logger.LogInformation("Retrieving profile for user {UserId}", userId);

            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found", userId);
                throw new KeyNotFoundException("User not found");
            }

            _logger.LogInformation("Retrieved profile for user {UserId}", user.Id);

            return new GetProfileResponse(user.Id, user.FirstName, user.LastName, user.Email);
        }
    }
}
