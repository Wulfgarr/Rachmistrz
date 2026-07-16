using Microsoft.EntityFrameworkCore;
using Rachmistrz.Web.Data;
using Rachmistrz.Web.DTOs;
using Rachmistrz.Web.Enums;
using Rachmistrz.Web.Models;

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

        public async Task<InvoiceDetailsDto?> GetInvoiceDetailsAsync(int id)
        {
            return await _dbContext.Invoices
                .AsNoTracking()
                .Where(invoice => invoice.Id == id)
                .Select(invoice => new InvoiceDetailsDto
                {
                    Id = invoice.Id,
                    InvoiceNumber = invoice.InvoiceNumber,
                    
                    SupplierName = invoice.Supplier.Name,
                    SupplierNip = invoice.Supplier.Nip,
                    
                    BranchName = invoice.Branch.Name,
                    BranchCode = invoice.Branch.Code,

                    CostCategoryName = invoice.CostCategory.Name,

                    IssueDate = invoice.IssueDate,
                    ReceivedDate = invoice.ReceivedDate,
                    DueDate = invoice.DueDate,

                    NetAmount = invoice.NetAmount,
                    VatAmount = invoice.VatAmount,
                    GrossAmount = invoice.GrossAmount,

                    Status = invoice.Status,
                    Description = invoice.Description,
                    
                    CreatedByUserEmail = invoice.CreatedByUser.Email ?? string.Empty,
                    CreatedAt = invoice.CreatedAt,
                    UpdatedAt = invoice.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateInvoiceAsync(
            CreateInvoiceDto dto,
            string createdByUserId)
        {
            if (dto.SupplierId is null)
            {
                throw new InvalidOperationException("Supplier is required.");
            }

            if (dto.BranchId is null)
            {
                throw new InvalidOperationException("Branch is required.");
            }

            if (dto.CostCategoryId is null)
            {
                throw new InvalidOperationException("Cost category is required.");
            }

            if (dto.IssueDate is null)
            {
                throw new InvalidOperationException("Issue date is required.");
            }

            if (dto.ReceivedDate is null)
            {
                throw new InvalidOperationException("Received date is required.");
            }

            if (dto.DueDate is null)
            {
                throw new InvalidOperationException("Due date is required.");
            }

            if (dto.NetAmount is null)
            {
                throw new InvalidOperationException("Net amount is required.");
            }

            if (dto.VatAmount is null)
            {
                throw new InvalidOperationException("Vat amount is required.");
            }

            if (dto.GrossAmount is null)
            {
                throw new InvalidOperationException("Gross amount is required.");
            }

            var invoice = new Invoice
            {
                InvoiceNumber = dto.InvoiceNumber,
                SupplierId = dto.SupplierId.Value,
                BranchId = dto.BranchId.Value,
                CostCategoryId = dto.CostCategoryId.Value,
                IssueDate = dto.IssueDate.Value,
                ReceivedDate = dto.ReceivedDate.Value,
                DueDate = dto.DueDate.Value,
                NetAmount = dto.NetAmount.Value,
                VatAmount = dto.VatAmount.Value,
                GrossAmount = dto.GrossAmount.Value,
                Description = dto.Description,
                Status = InvoiceStatus.Draft,
                CreatedByUserId = createdByUserId,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Invoices.Add(invoice);

            await _dbContext.SaveChangesAsync();

            return invoice.Id;
        }


    }
}
