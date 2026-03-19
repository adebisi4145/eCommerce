using MediatR;

namespace eCommerce.Features.Users.Profile
{
    public record GetProfileQuery(Guid UserId) : IRequest<GetProfileResponse>;
}
