using eCommerce.Infrastructure.Data;
using eCommerce.Infrastructure.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Users.Login
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ITokenService _tokenService;

        public LoginUserHandler(ECommerceDbContext dbContext, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        public async Task<LoginUserResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var email = command.Email.ToLowerInvariant();

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user == null)
                throw new InvalidOperationException("Invalid email or password");

            var verifyPassword = BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash);
            if (!verifyPassword)
                throw new InvalidOperationException("Invalid email or password");

            var token = _tokenService.GenerateToken(user);

            return new LoginUserResponse(token, user.Id, user.Email);
        }
    }
}
