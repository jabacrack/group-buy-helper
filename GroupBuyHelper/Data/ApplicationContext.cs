using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GroupBuyHelper.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductList> ProductLists { get; set; }
        public DbSet<UserOrderItem> UserOrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductList>()
                .HasMany(x => x.Products)
                .WithOne(x => x.ProductList)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<ProductList>()
                .HasMany(x => x.OrderItems)
                .WithOne(x => x.ProductList)
                .HasForeignKey(x => x.ProductListId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<ApplicationUser>()
                .HasMany(x => x.Orders)
                .WithOne(x => x.Owner)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Product>()
                .HasMany(x => x.ConnectedOrderItems)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
