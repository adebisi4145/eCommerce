using MediatR;

namespace eCommerce.Features.Users.Profile
{
    public record GetProfileQuery() : IRequest<GetProfileResponse>;
}
