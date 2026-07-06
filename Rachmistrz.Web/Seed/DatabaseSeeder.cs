using Microsoft.AspNetCore.Identity;
using Rachmistrz.Web.Constants;
using Rachmistrz.Web.Data;
using Rachmistrz.Web.Models;
using Microsoft.EntityFrameworkCore;


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

        public static async Task SeedBusinessDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            await SeedBranchesAsync(dbContext);
            await SeedSuppliersAsync(dbContext);
            await SeedCostCategoriesAsync(dbContext);
        }

        private static async Task SeedBranchesAsync(ApplicationDbContext dbContext)
        {
            if (await dbContext.Branches.AnyAsync())
            {
                return;
            }

            var branches = new List<Branch>
            {
                new()
                {
                    Name = "Centrum Logistyki Kraków",
                    Code = "KRK-01",
                    City = "Kraków",
                    Address = "ul. Logistyczna 10",
                    IsActive = true
                },
                new()
                {
                    Name = "Oddział Warszawa Śródmieście",
                    Code = "WAW-01",
                    City = "Warszawa",
                    Address = "ul. Pocztowa 15",
                    IsActive = true
                },
                new()
                {
                    Name = "Oddział Gdańsk",
                    Code = "GDN-01",
                    City = "Gdańsk",
                    Address = "ul. Morska 22",
                    IsActive = true
                }
            };
            await dbContext.Branches.AddRangeAsync(branches);
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedSuppliersAsync(ApplicationDbContext dbContext)
        {
            if (await dbContext.Suppliers.AnyAsync())
            {
                return;
            }

            var suppliers = new List<Supplier>
            {
                new()
                {
                    Name = "Energia Polska S.A.",
                    Nip = "1234567890",
                    Email = "kontakt@energiapolska.pl",
                    Phone = "123456789",
                    Address = "ul. Energetyczna 1, Warszawa",
                    IsActive = true
                },
                new()
                {
                    Name = "LogiTrans Sp. z o.o.",
                    Nip = "9876543210",
                    Email = "biuro@logitrans.pl",
                    Phone = "987654321",
                    Address = "ul. Transportowa 5, Kraków",
                    IsActive = true
                },
                new()
                {
                    Name = "OfficeMarket Sp. z o.o.",
                    Nip = "5554443322",
                    Email = "sprzedaz@officemarket.pl",
                    Phone = "555444333",
                    Address = "ul. Biurowa 7, Poznań",
                    IsActive = true
                }
            };

            await dbContext.Suppliers.AddRangeAsync(suppliers);
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedCostCategoriesAsync(ApplicationDbContext dbContext)
        {
            if (await dbContext.CostCategories.AnyAsync())
            {
                return;
            }

            var categories = new List<CostCategory>
    {
        new()
        {
            Name = "Transport",
            Description = "Koszty usług transportowych i logistycznych.",
            IsActive = true
        },
        new()
        {
            Name = "Energia",
            Description = "Koszty energii elektrycznej i mediów.",
            IsActive = true
        },
        new()
        {
            Name = "Materiały biurowe",
            Description = "Zakup materiałów i wyposażenia biurowego.",
            IsActive = true
        },
        new()
        {
            Name = "Usługi informatyczne",
            Description = "Koszty usług IT, licencji i wsparcia technicznego.",
            IsActive = true
        },
        new()
        {
            Name = "Utrzymanie budynków",
            Description = "Koszty remontów, sprzątania i utrzymania placówek.",
            IsActive = true
        }
    };

            await dbContext.CostCategories.AddRangeAsync(categories);
            await dbContext.SaveChangesAsync();
        }
    }
}
