using Microsoft.EntityFrameworkCore;
using Rachmistrz.Web.Constants;
using Rachmistrz.Web.Data;
using Rachmistrz.Web.DTOs;
using Rachmistrz.Web.Enums;
using Rachmistrz.Web.Models;

namespace Rachmistrz.Web.Services
{
    public class ReportService
    {
        private readonly ApplicationDbContext _dbContext;

        public ReportService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MonthlyReportDto> GetMonthlyReportAsync(
            int year,
            int month,
            string userId,
            int? userBranchId,
            IEnumerable<string> roles)
        {
            var monthStart = new DateTime(year, month, 1);
            var nextMonthStart = monthStart.AddMonths(1);
            var today = DateTime.Today;

            var query = _dbContext.Invoices
                .AsNoTracking()
                .Where(invoice =>
                    invoice.IssueDate >= monthStart &&
                    invoice.IssueDate < nextMonthStart)
                .AsQueryable();

            query = ApplyInvoiceVisibilityRules(
                query,
                userId,
                userBranchId,
                roles);

            var invoices = await query
                .OrderBy(invoice => invoice.IssueDate)
                .Select(invoice => new MonthlyReportInvoiceDto
                {
                    Id = invoice.Id,
                    InvoiceNumber = invoice.InvoiceNumber,
                    SupplierName = invoice.Supplier.Name,
                    BranchName = invoice.Branch.Name,
                    IssueDate = invoice.IssueDate,
                    DueDate = invoice.DueDate,
                    NetAmount = invoice.NetAmount,
                    VatAmount = invoice.VatAmount,
                    GrossAmount = invoice.GrossAmount,
                    Status = invoice.Status
                })
                .ToListAsync();

            return new MonthlyReportDto
            {
                Year = year,
                Month = month,
                InvoicesCount = invoices.Count,
                TotalNetAmount = invoices.Sum(invoice => invoice.NetAmount),
                TotalVatAmount = invoices.Sum(invoice => invoice.VatAmount),
                TotalGrossAmount = invoices.Sum(invoice => invoice.GrossAmount),
                PaidInvoicesCount = invoices.Count(invoice =>
                    invoice.Status == InvoiceStatus.Paid),
                UnpaidInvoicesCount = invoices.Count(invoice =>
                    invoice.Status != InvoiceStatus.Paid &&
                    invoice.Status != InvoiceStatus.Cancelled),
                OverdueInvoicesCount = invoices.Count(invoice =>
                    invoice.DueDate < today &&
                    invoice.Status != InvoiceStatus.Paid &&
                    invoice.Status != InvoiceStatus.Cancelled),
                Invoices = invoices
            };
        }

        private static IQueryable<Invoice> ApplyInvoiceVisibilityRules(
            IQueryable<Invoice> query,
            string userId,
            int? userBranchId,
            IEnumerable<string> roles)
        {
            if (roles.Contains(RoleNames.Admin) || roles.Contains(RoleNames.Accounting))
            {
                return query;
            }

            if (roles.Contains(RoleNames.BranchManager) && userBranchId is not null)
            {
                return query.Where(invoice => invoice.BranchId == userBranchId.Value);
            }

            if (roles.Contains(RoleNames.Employee))
            {
                return query.Where(invoice => invoice.CreatedByUserId == userId);
            }

            return query.Where(invoice => false);
        }
    }
}
