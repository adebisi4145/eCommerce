using MediatR;

namespace eCommerce.Features.Users.AddAddress
{
    public record AddAddressCommand(string Street, string City, string State, string Country, string ZipCode, bool IsDefault) : IRequest<AddAddressResponse>;
}
