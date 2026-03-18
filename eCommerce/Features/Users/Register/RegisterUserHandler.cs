using eCommerce.Domain.Entities;
using eCommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Features.Users.Register
{
    public class RegisterUserHandler: IRequestHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly ECommerceDbContext _dbContext;
        public RegisterUserHandler(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var email = command.Email.ToLowerInvariant();

            var exists = await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
            if (exists)
                throw new Exception("Email already exists.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);

            var user = new User( email, passwordHash, command.FirstName, command.LastName);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new RegisterUserResponse(user.Id);
        }
    }
}
