using Microsoft.EntityFrameworkCore;
using System;
using eCommerce.Domain.Entities;
using eCommerce.Domain.Enums;
using eCommerce.Domain.Enums;

namespace eCommerce.Infrastructure.Data
{
    public class ECommerceDbContext: DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options): base(options) 
        { 
        
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>(o =>
            {
                o.HasKey(x => x.Id);

                o.Property(x => x.Status)
                    .HasConversion<string>();

                o.OwnsMany(x => x.Items, i =>
                {
                    i.WithOwner().HasForeignKey("OrderId");
                    i.Property<Guid>("Id");
                    i.HasKey("Id");
                    i.Ignore(x => x.Subtotal);
                });

                o.Navigation(nameof(Order.Items))
                    .UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            builder.Entity<Cart>(c =>
            {
                c.HasKey(x => x.Id);
                c.HasIndex(x => x.UserId).IsUnique();

                c.OwnsMany(x => x.Items, i =>
                {
                    i.WithOwner().HasForeignKey("CartId");
                    i.Property<Guid>("Id");
                    i.HasKey("Id");
                });

                c.Navigation(nameof(Cart.Items))
                    .UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            builder.Entity<User>(u =>
            {
                u.HasKey(x => x.Id);
                u.HasIndex(x => x.Email).IsUnique();

                u.OwnsMany(x => x.Addresses, a =>
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
        public DbSet<Product> Products { get;set; }
        public DbSet<Category> Categories { get;set; }
        public DbSet<Cart> Carts { get;set; }
        public DbSet<Order> Orders { get;set; }
    }
}
