using Microsoft.AspNetCore.Identity;
using Rachmistrz.Web.Constants;
using Rachmistrz.Web.Data;


namespace Rachmistrz.Web.Seed
{
    public class DatabaseSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles =
                [
                    RoleNames.Admin,
                    RoleNames.Accounting,
                    RoleNames.BranchManager,
                    RoleNames.Employee
                ];

            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

        }

        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            const string adminEmail = "admin@rachmistrz.local";
            const string adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser is null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "System",
                    LastName = "Administrator"
                };

                var createResult = await userManager.CreateAsync(
                    adminUser,
                    adminPassword);

                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Failed to create default admin user.");
                }
            }

            var isInAdminRole = await userManager.IsInRoleAsync(
                adminUser,
                RoleNames.Admin);

            if (!isInAdminRole)
            {
                var addToRoleResult = await userManager.AddToRoleAsync(
                    adminUser,
                    RoleNames.Admin);

                if (!addToRoleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Failed to assign Admin role to default admin user.");
                }
            }
        }
    }
}
