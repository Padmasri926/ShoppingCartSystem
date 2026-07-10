using EshoppingZone.Models;
using Microsoft.AspNetCore.Identity;

namespace EshoppingZone.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(UserManager<User> userManager)
        {
            var adminEmail = "admin@gmail.com";
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            
            if (existingAdmin == null)
            {
                var admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Role = "Admin",
                    IsApproved = true
                };

                await userManager.CreateAsync(admin, "Admin@123");
            }
        }
    }
}
