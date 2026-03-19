using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eCommerce.Features.Users.Profile
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery, GetProfileResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<GetProfileHandler> _logger;

        public GetProfileHandler(ECommerceDbContext dbContext, ILogger<GetProfileHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<GetProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving profile for UserId {UserId}", request.UserId);

            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found", request.UserId);
                throw new KeyNotFoundException("User not found");
            }

            _logger.LogInformation("Retrieved profile for UserId {UserId}", user.Id);

            return new GetProfileResponse(user.Id, user.FirstName, user.LastName, user.Email);
        }
    }
}
