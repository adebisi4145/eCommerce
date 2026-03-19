using eCommerce.Infrastructure.Data;
using eCommerce.Infrastructure.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eCommerce.Features.Users.Login
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginUserHandler> _logger;

        public LoginUserHandler(ECommerceDbContext dbContext, ITokenService tokenService, ILogger<LoginUserHandler> logger)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<LoginUserResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var email = command.Email.ToLowerInvariant();

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Invalid login attempt for email {Email}", email);
                throw new InvalidOperationException("Invalid email or password");
            }

            var verifyPassword = BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash);
            if (!verifyPassword)
            {
                _logger.LogWarning("Invalid login attempt for email {Email}", email);
                throw new InvalidOperationException("Invalid email or password");
            }

            var token = _tokenService.GenerateToken(user);

            _logger.LogInformation("User {UserId} logged in successfully", user.Id);
            return new LoginUserResponse(token, user.Id, user.Email);
        }
    }
}
