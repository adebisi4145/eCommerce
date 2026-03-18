using MediatR;

namespace eCommerce.Features.Users.Login
{
    public record LoginUserCommand(string Email, string Password) : IRequest<LoginUserResponse>;
}
