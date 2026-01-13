using Store.Core.Models;
using Store.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add MVC
builder.Services.AddControllersWithViews();

// Register DbContext with Identity
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (builder.Environment.IsDevelopment() && connectionString != null && connectionString.Contains(".db"))
{
    // Use SQLite for development
    builder.Services.AddDbContext<StoreDbContext>(options =>
        options.UseSqlite(connectionString));
}
else
{
    // Use SQL Server for production
    builder.Services.AddDbContext<StoreDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<StoreDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
    
    // Ensure database is created
    context.Database.EnsureCreated();
    
    // Seed data if database is empty (only if not using HasData seed)
    if (!context.Products.Any())
    {
        var electronics = new Category { Name = "Electronics" };
        var books = new Category { Name = "Books" };

        context.Categories.AddRange(electronics, books);

        context.Products.AddRange(
            new Product { Name = "Laptop", Description = "15-inch ultrabook", Price = 1200, Category = electronics },
            new Product { Name = "Smartphone", Description = "Latest model smartphone", Price = 800, Category = electronics },
            new Product { Name = "Novel", Description = "Bestselling fiction book", Price = 20, Category = books }
        );

        context.SaveChanges();
    }
}

app.Run();