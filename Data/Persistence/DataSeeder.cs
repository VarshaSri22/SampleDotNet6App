using SampleDotNet6App.Domain.Models;
using SampleDotNet6App.Domain.Security;

namespace SampleDotNet6App.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User { Username = "admin", PasswordHash = PasswordHasher.Hash("Admin123!"), Role = UserRole.Admin },
                new User { Username = "jane.smith", PasswordHash = PasswordHasher.Hash("Manager123!"), Role = UserRole.Manager },
                new User { Username = "john.doe", PasswordHash = PasswordHasher.Hash("User123!"), Role = UserRole.User }
            );
        }

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Name = "Wireless Mouse", Category = "Accessories", Price = 19.99m, StockQuantity = 150 },
                new Product { Name = "Mechanical Keyboard", Category = "Accessories", Price = 89.99m, StockQuantity = 60 },
                new Product { Name = "27-inch Monitor", Category = "Displays", Price = 249.99m, StockQuantity = 25 }
            );
        }

        await context.SaveChangesAsync();
    }
}
