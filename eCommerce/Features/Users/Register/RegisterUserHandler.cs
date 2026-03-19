using eCommerce.Domain.Entities;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eCommerce.Features.Users.Register
{
    public class RegisterUserHandler: IRequestHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(ECommerceDbContext dbContext, ILogger<RegisterUserHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var email = command.Email.ToLowerInvariant();

            _logger.LogInformation("Registering user with email {Email}", email);

            var exists = await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
            if (exists)
            {
                _logger.LogWarning("Registration attempt with existing email {Email}", email);
                throw new Exception("Email already exists.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);

            var user = new User( email, passwordHash, command.FirstName, command.LastName);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {UserId} registered successfully", user.Id);

            return new RegisterUserResponse(user.Id);
        }
    }
}
