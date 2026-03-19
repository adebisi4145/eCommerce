namespace eCommerce.Features.Users.Profile
{
    public record GetProfileResponse(Guid Id, string FirstName, string LastName, string Email);
}
