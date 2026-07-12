using Microsoft.EntityFrameworkCore;
using Rachmistrz.Web.Data;
using Rachmistrz.Web.DTOs;

namespace Rachmistrz.Web.Services
{
    public class InvoiceService
    {
        private readonly ApplicationDbContext _dbContext;

        public InvoiceService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<InvoiceListItemDto>> GetInvoicesAsync()
        {
            return await _dbContext.Invoices
                .AsNoTracking()
                .OrderByDescending(invoice => invoice.CreatedAt)
                .Select(invoice => new InvoiceListItemDto
                {
                    Id = invoice.Id,
                    InvoiceNumber = invoice.Supplier.Name,
                    SupplierName = invoice.Supplier.Name,
                    BranchName = invoice.Branch.Name,
                    CostCategoryName = invoice.CostCategory.Name,
                    GrossAmount = invoice.GrossAmount,
                    DueDate = invoice.DueDate,
                    Status = invoice.Status
                })
                .ToListAsync();
        }
    }
}
