using eCommerce.Domain.Entities;

namespace eCommerce.Infrastructure.Auth
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
