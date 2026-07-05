using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rachmistrz.Web.Models;

namespace Rachmistrz.Web.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Invoice> Invoices => Set<Invoice>();

        public DbSet<Supplier> Suppliers => Set<Supplier>();

        public DbSet<Branch> Branches => Set<Branch>();

        public DbSet<CostCategory> CostCategories => Set<CostCategory>();

        public DbSet<InvoiceComment> InvoiceComments => Set<InvoiceComment>();

        public DbSet<InvoiceAuditLog> InvoiceAuditLogs => Set<InvoiceAuditLog>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureApplicationUser(builder);
            ConfigureBranch(builder);
            ConfigureSupplier(builder);
            ConfigureCostCategory(builder);
            ConfigureInvoice(builder);
            ConfigureInvoiceComment(builder);
            ConfigureInvoiceAuditLog(builder);
        }

        private static void ConfigureBranch(ModelBuilder builder)
        {
            builder.Entity<Branch>(entity =>
            {
                entity.Property(branch => branch.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(branch => branch.Code)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(branch => branch.City)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(branch => branch.Address)
                    .HasMaxLength(250)
                    .IsRequired();

                entity.HasIndex(branch => branch.Code)
                    .IsUnique();
            });
        }

        private static void ConfigureSupplier(ModelBuilder builder)
        {
            builder.Entity<Supplier>(entity =>
            {
                entity.Property(supplier => supplier.Name)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(supplier => supplier.Nip)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(supplier => supplier.Email)
                    .HasMaxLength(200);

                entity.Property(supplier => supplier.Phone)
                    .HasMaxLength(30);

                entity.Property(supplier => supplier.Address)
                    .HasMaxLength(250);

                entity.HasIndex(supplier => supplier.Nip)
                    .IsUnique();
            });
        }

        private static void ConfigureCostCategory(ModelBuilder builder)
        {
            builder.Entity<CostCategory>(entity =>
            {
                entity.Property(category => category.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(category => category.Description)
                    .HasMaxLength(500);

                entity.HasIndex(category => category.Name)
                    .IsUnique();
            });
        }

        private static void ConfigureInvoice(ModelBuilder builder)
        {
            builder.Entity<Invoice>(entity =>
            {
                entity.Property(invoice => invoice.InvoiceNumber)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(invoice => invoice.Description)
                    .HasMaxLength(1000);

                entity.Property(invoice => invoice.NetAmount)
                    .HasPrecision(18, 2);

                entity.Property(invoice => invoice.VatAmount)
                    .HasPrecision(18, 2);

                entity.Property(invoice => invoice.GrossAmount)
                    .HasPrecision(18, 2);

                entity.HasOne(invoice => invoice.Supplier)
                    .WithMany(supplier => supplier.Invoices)
                    .HasForeignKey(invoice => invoice.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(invoice => invoice.Branch)
                    .WithMany(branch => branch.Invoices)
                    .HasForeignKey(invoice => invoice.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(invoice => invoice.CostCategory)
                    .WithMany(category => category.Invoices)
                    .HasForeignKey(invoice => invoice.CostCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(invoice => invoice.CreatedByUser)
                    .WithMany(user => user.CreatedInvoices)
                    .HasForeignKey(invoice => invoice.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureInvoiceComment(ModelBuilder builder)
        {
            builder.Entity<InvoiceComment>(entity =>
            {
                entity.Property(comment => comment.Content)
                    .HasMaxLength(2000)
                    .IsRequired();

                entity.HasOne(comment => comment.Invoice)
                    .WithMany(invoice => invoice.Comments)
                    .HasForeignKey(comment => comment.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(comment => comment.User)
                    .WithMany(user => user.InvoiceComments)
                    .HasForeignKey(comment => comment.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureInvoiceAuditLog(ModelBuilder builder)
        {
            builder.Entity<InvoiceAuditLog>(entity =>
            {
                entity.Property(log => log.Action)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(log => log.Description)
                    .HasMaxLength(1000);

                entity.HasOne(log => log.Invoice)
                    .WithMany(invoice => invoice.AuditLogs)
                    .HasForeignKey(log => log.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(log => log.User)
                    .WithMany(user => user.InvoiceAuditLogs)
                    .HasForeignKey(log => log.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureApplicationUser(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(user => user.FirstName)
                    .HasMaxLength(100);

                entity.Property(user => user.LastName)
                    .HasMaxLength(100);

                entity.HasOne(user => user.Branch)
                    .WithMany(branch => branch.Users)
                    .HasForeignKey(user => user.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
