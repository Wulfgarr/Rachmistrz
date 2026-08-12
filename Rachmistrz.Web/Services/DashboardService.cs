using Microsoft.EntityFrameworkCore;
using Rachmistrz.Web.Constants;
using Rachmistrz.Web.Data;
using Rachmistrz.Web.DTOs;
using Rachmistrz.Web.Enums;

namespace Rachmistrz.Web.Services
{
    public class DashboardService
    {
        private readonly ApplicationDbContext _dbContext;

        public DashboardService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync(
            string userId,
            int? userBranchId,
            IEnumerable<string> roles)
        {
            var query = _dbContext.Invoices
                // Dashboard tylko czyta dane, więc nie potrzebujemy śledzenia zmian przez EF Core.
                .AsNoTracking()
                .AsQueryable();

            query = ApplyInvoiceVisibilityRules(
                query,
                userId,
                userBranchId,
                roles);

            var today = DateTime.Today;

            var currentMonthStart = new DateTime(today.Year, today.Month, 1);
            var nextMonthStart = currentMonthStart.AddMonths(1);

            return new DashboardStatsDto
            {
                TotalInvoicesCount = await query.CountAsync(),

                DraftInvoicesCount = await query.CountAsync(invoice =>
                    invoice.Status == InvoiceStatus.Draft),

                SubmittedInvoicesCount = await query.CountAsync(invoice =>
                    invoice.Status == InvoiceStatus.Submitted),

                UnderReviewInvoicesCount = await query.CountAsync(invoice =>
                    invoice.Status == InvoiceStatus.UnderReview),

                ApprovedInvoicesCount = await query.CountAsync(invoice =>
                    invoice.Status == InvoiceStatus.Approved),

                BookedInvoicesCount = await query.CountAsync(invoice =>
                    invoice.Status == InvoiceStatus.Booked),

                PaidInvoicesCount = await query.CountAsync(invoice =>
                    invoice.Status == InvoiceStatus.Paid),

                OverdueInvoicesCount = await query.CountAsync(invoice =>
                    invoice.DueDate < today &&
                    invoice.Status != InvoiceStatus.Paid &&
                    invoice.Status != InvoiceStatus.Cancelled),

                CurrentMonthGrossAmount = await query
                    .Where(invoice =>
                        invoice.IssueDate >= currentMonthStart &&
                        invoice.IssueDate < nextMonthStart)
                    .SumAsync(invoice => invoice.GrossAmount)
            };
        }

        private static IQueryable<Models.Invoice> ApplyInvoiceVisibilityRules(
            IQueryable<Models.Invoice> query,
            string userId,
            int? userBranchId,
            IEnumerable<string> roles)
        {
            if (roles.Contains(RoleNames.Admin) || roles.Contains(RoleNames.Accounting))
            {
                // Admin i księgowość widzą statystyki ze wszystkich faktur.
                return query;
            }

            if (roles.Contains(RoleNames.BranchManager) && userBranchId is not null)
            {
                // Kierownik widzi statystyki tylko ze swojego oddziału.
                return query.Where(invoice => invoice.BranchId == userBranchId.Value);
            }

            if (roles.Contains(RoleNames.Employee))
            {
                // Pracownik widzi statystyki tylko ze swoich faktur.
                return query.Where(invoice => invoice.CreatedByUserId == userId);
            }

            // Jeśli użytkownik nie ma żadnej obsługiwanej roli, nie pokazujemy mu żadnych danych.
            return query.Where(invoice => false);
        }
    }
}
