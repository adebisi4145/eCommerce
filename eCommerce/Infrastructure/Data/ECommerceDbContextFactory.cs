using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace eCommerce.Infrastructure.Data
{
    // Design-time factory for EF Core tools
    public class ECommerceDbContextFactory : IDesignTimeDbContextFactory<ECommerceDbContext>
    {
        public ECommerceDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ECommerceDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ECommerceDbContext(optionsBuilder.Options);
        }
    }
}