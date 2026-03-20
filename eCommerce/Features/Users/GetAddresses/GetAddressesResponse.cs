namespace eCommerce.Features.Users.GetAddresses
{
    public record GetAddressesResponse(Guid Id, string Street, string City, string State, string Country, string ZipCode, bool IsDefault);
}
