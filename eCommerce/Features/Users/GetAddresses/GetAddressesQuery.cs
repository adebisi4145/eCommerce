using MediatR;

namespace eCommerce.Features.Users.GetAddresses
{
    public record GetAddressesQuery() : IRequest<List<GetAddressesResponse>>;
}
