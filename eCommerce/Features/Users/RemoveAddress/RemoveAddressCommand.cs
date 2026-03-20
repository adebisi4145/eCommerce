using MediatR;

namespace eCommerce.Features.Users.RemoveAddress
{
    public record RemoveAddressCommand(Guid AddressId) : IRequest;
}
