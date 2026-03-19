using eCommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using eCommerce.Infrastructure.Auth;

namespace eCommerce.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ECommerceDbContext>(options => options.UseSqlServer(connectionString));

            services.AddSingleton<ITokenService, TokenService>();

            return services;
        }
    }
}
