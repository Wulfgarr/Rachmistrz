using Microsoft.AspNetCore.Identity;
using Rachmistrz.Web.Constants;


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
    }
}
