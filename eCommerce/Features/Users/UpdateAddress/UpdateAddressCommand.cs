using MediatR;

namespace eCommerce.Features.Users.UpdateAddress
{
    public record UpdateAddressCommand(Guid AddressId, string Street, string City, string State, string Country, string ZipCode) : IRequest;
}
