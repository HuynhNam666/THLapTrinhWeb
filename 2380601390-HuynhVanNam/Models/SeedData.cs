using Microsoft.AspNetCore.Identity;

namespace _2380601390_HuynhVanNam.Models
{
    public class SeedData
    {
        public static async Task SeedAdmin(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 👉 Tạo role
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            // 👉 Admin
            var admin = await userManager.FindByEmailAsync("admin@gmail.com");
            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // 👉 User thường
            var user = await userManager.FindByEmailAsync("user@gmail.com");
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = "user@gmail.com",
                    Email = "user@gmail.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "123456");
                await userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}