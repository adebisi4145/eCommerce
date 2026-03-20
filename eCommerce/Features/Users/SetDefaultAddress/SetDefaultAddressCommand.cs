using MediatR;

namespace eCommerce.Features.Users.SetDefaultAddress
{
    public record SetDefaultAddressCommand(Guid AddressId) : IRequest;
}
