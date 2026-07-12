using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rachmistrz.Web.Constants;
using Rachmistrz.Web.Data;
using Rachmistrz.Web.Enums;
using Rachmistrz.Web.Models;

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

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            await SeedBranchesAsync(dbContext);
            await SeedSuppliersAsync(dbContext);
            await SeedCostCategoriesAsync(dbContext);
            await SeedInvoiceAsync(dbContext, userManager);
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

        private static async Task SeedInvoiceAsync(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            if (await dbContext.Invoices.AnyAsync())
            {
                return;
            }

            var adminUser = await userManager.FindByEmailAsync("admin@rachmistrz.local");

            if (adminUser is null)
            {
                throw new InvalidOperationException(
                    "Default admin user must exist before seeding invoices");
            }


            // Branches
            var krakowBranch = await dbContext.Branches
                .SingleAsync(branch => branch.Code == "KRK-01");

            var warsawBranch = await dbContext.Branches
                .SingleAsync(branch => branch.Code == "WAW-01");

            var gdanskBranch = await dbContext.Branches
                .SingleAsync(branch => branch.Code == "GDN-01");

            // Suppliers
            var energiaSupplier = await dbContext.Suppliers
                .SingleAsync(supplier => supplier.Nip == "1234567890");

            var logiTransSupplier = await dbContext.Suppliers
                .SingleAsync(supplier => supplier.Nip == "9876543210");

            var officeMarketSupplier = await dbContext.Suppliers
                .SingleAsync(supplier => supplier.Nip == "5554443322");

            // Cost category
            var transportCategory = await dbContext.CostCategories
                .SingleAsync(category => category.Name == "Transport");

            var energyCategory = await dbContext.CostCategories
                .SingleAsync(category => category.Name == "Energia");

            var officeCategory = await dbContext.CostCategories
                .SingleAsync(category => category.Name == "Materiały biurowe");


            var invoices = new List<Invoice>
            {
                new()
                {
                    InvoiceNumber = "FV/2026/001",
                    SupplierId = energiaSupplier.Id,
                    BranchId = krakowBranch.Id,
                    CostCategoryId = energyCategory.Id,
                    IssueDate = DateTime.Today.AddDays(-20),
                    ReceivedDate = DateTime.Today.AddDays(-18),
                    DueDate = DateTime.Today.AddDays(10),
                    NetAmount = 2500.00m,
                    VatAmount = 575.00m,
                    GrossAmount = 3075.00m,
                    Status = Enums.InvoiceStatus.Submitted,
                    Description = "Faktura za energię elektryczną dla oddziału Kraków",
                    CreatedByUserId = adminUser.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-18)
                },
                new()
                {
                    InvoiceNumber = "FV/2026/002",
                    SupplierId = logiTransSupplier.Id,
                    BranchId = warsawBranch.Id,
                    CostCategoryId = transportCategory.Id,
                    IssueDate = DateTime.Today.AddDays(-15),
                    ReceivedDate = DateTime.Today.AddDays(-14),
                    DueDate = DateTime.Today.AddDays(5),
                    NetAmount = 4200.00m,
                    VatAmount = 966.00m,
                    GrossAmount = 5166.00m,
                    Status = InvoiceStatus.UnderReview,
                    Description = "Usługi transportowe dla oddziału Warszawa.",
                    CreatedByUserId = adminUser.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-14)
                },
                new()
                {
                    InvoiceNumber = "FV/2026/003",
                    SupplierId = officeMarketSupplier.Id,
                    BranchId = gdanskBranch.Id,
                    CostCategoryId = officeCategory.Id,
                    IssueDate = DateTime.Today.AddDays(-10),
                    ReceivedDate = DateTime.Today.AddDays(-9),
                    DueDate = DateTime.Today.AddDays(20),
                    NetAmount = 850.00m,
                    VatAmount = 195.50m,
                    GrossAmount = 1045.50m,
                    Status = InvoiceStatus.Approved,
                    Description = "Zakup materiałów biurowych dla oddziału Gdańsk.",
                    CreatedByUserId = adminUser.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-9)
                },
                new()
                {
                    InvoiceNumber = "FV/2026/004",
                    SupplierId = logiTransSupplier.Id,
                    BranchId = krakowBranch.Id,
                    CostCategoryId = transportCategory.Id,
                    IssueDate = DateTime.Today.AddDays(-30),
                    ReceivedDate = DateTime.Today.AddDays(-29),
                    DueDate = DateTime.Today.AddDays(-2),
                    NetAmount = 1200.00m,
                    VatAmount = 276.00m,
                    GrossAmount = 1476.00m,
                    Status = InvoiceStatus.Paid,
                    Description = "Zrealizowana i opłacona faktura transportowa.",
                    CreatedByUserId = adminUser.Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-29)
                }
            };

            await dbContext.Invoices.AddRangeAsync(invoices);
            await dbContext.SaveChangesAsync();
        }
    }
}
