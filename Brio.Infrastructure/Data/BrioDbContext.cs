using Store.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Store.Infrastructure.Data
{
    public class StoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" }
            );

            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "15-inch ultrabook", Price = 1200, CategoryId = 1 },
                new Product { Id = 2, Name = "Smartphone", Description = "Latest model smartphone", Price = 800, CategoryId = 1 },
                new Product { Id = 3, Name = "Novel", Description = "Bestselling fiction book", Price = 20, CategoryId = 2 }
            );
        }
    }
}