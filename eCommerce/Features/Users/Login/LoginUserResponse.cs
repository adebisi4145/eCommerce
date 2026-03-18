namespace eCommerce.Features.Users.Login
{
    public record LoginUserResponse(string Token, Guid UserId, string Email);
}
