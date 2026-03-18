using Microsoft.EntityFrameworkCore;
using eCommerce.Domain.Entities;

namespace eCommerce.Infrastructure.Data
{
    public class ECommerceDbContext: DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options): base(options) 
        { 
        
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(u =>
            {
                u.HasKey(x => x.Id);
                u.HasIndex(x => x.Email).IsUnique();

                u.OwnsMany(typeof(Address), "_addresses", a =>
                {
                    a.WithOwner().HasForeignKey("UserId");
                    a.Property<Guid>("Id");
                    a.HasKey("Id");
                });

                u.Navigation(nameof(User.Addresses))
                    .UsePropertyAccessMode(PropertyAccessMode.Field);
            });
        }
        public DbSet<User> Users { get;set; }
    }
}
