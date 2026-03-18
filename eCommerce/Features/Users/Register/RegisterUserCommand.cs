using MediatR;

namespace eCommerce.Features.Users.Register
{
    public record RegisterUserCommand( string FirstName, string LastName, string Email, string Password) : IRequest<RegisterUserResponse>;
}
