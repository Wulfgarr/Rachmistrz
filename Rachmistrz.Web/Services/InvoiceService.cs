using Microsoft.EntityFrameworkCore;
using Rachmistrz.Web.Data;
using Rachmistrz.Web.DTOs;
using Rachmistrz.Web.Enums;
using Rachmistrz.Web.Models;
using Rachmistrz.Web.Constants;

namespace Rachmistrz.Web.Services
{
    public class InvoiceService
    {
        private readonly ApplicationDbContext _dbContext;

        public InvoiceService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<InvoiceListItemDto>> GetInvoicesAsync(
            string userId,
            int? userBranchId,
            IEnumerable<string> roles)
        {

            var query = _dbContext.Invoices
                .AsNoTracking()
                .AsQueryable();

            if (roles.Contains(RoleNames.Admin) || roles.Contains(RoleNames.Accounting))
            {
                // Admin and accounting can see all invoices.
            }
            else if (roles.Contains(RoleNames.BranchManager) && userBranchId is not null)
            {
                query = query.Where(invoice => invoice.BranchId == userBranchId.Value);
            }
            else if (roles.Contains(RoleNames.Employee))
            {
                query = query.Where(invoice => invoice.CreatedByUserId == userId);
            }
            else
            {
                query = query.Where(invoice => false);
            }

            return await query
                .OrderByDescending(invoice => invoice.CreatedAt)
                .Select(invoice => new InvoiceListItemDto
                {
                    Id = invoice.Id,
                    InvoiceNumber = invoice.InvoiceNumber,
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

                    BranchId = invoice.BranchId,
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

                    CreatedByUserId = invoice.CreatedByUserId,
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

        public async Task<EditInvoiceDto?> GetInvoiceForEditAsync(int id)
        {
            return await _dbContext.Invoices
                .AsNoTracking()
                .Where(invoice => invoice.Id == id)
                .Select(invoice => new EditInvoiceDto
                {
                    Id = invoice.Id,
                    InvoiceNumber = invoice.InvoiceNumber,
                    SupplierId = invoice.SupplierId,
                    BranchId = invoice.BranchId,
                    CostCategoryId = invoice.CostCategoryId,
                    IssueDate = invoice.IssueDate,
                    ReceivedDate = invoice.ReceivedDate,
                    DueDate = invoice.DueDate,
                    NetAmount = invoice.NetAmount,
                    VatAmount = invoice.VatAmount,
                    GrossAmount = invoice.GrossAmount,
                    Description = invoice.Description
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateInvoiceAsync(EditInvoiceDto dto)
        {
            var invoice = await _dbContext.Invoices
                .FirstOrDefaultAsync(invoice => invoice.Id == dto.Id);

            if (invoice is null)
            {
                return false;
            }

            if (dto.SupplierId is null) throw new InvalidOperationException("Supplier is required.");
            if (dto.BranchId is null) throw new InvalidOperationException("Branch is required.");
            if (dto.CostCategoryId is null) throw new InvalidOperationException("Cost category is required.");
            if (dto.IssueDate is null) throw new InvalidOperationException("Issue date is required.");
            if (dto.ReceivedDate is null) throw new InvalidOperationException("Received date is required.");
            if (dto.DueDate is null) throw new InvalidOperationException("Due date is required.");

            invoice.InvoiceNumber = dto.InvoiceNumber;
            invoice.SupplierId = dto.SupplierId.Value;
            invoice.BranchId = dto.BranchId.Value;
            invoice.CostCategoryId = dto.CostCategoryId.Value;
            invoice.IssueDate = dto.IssueDate.Value;
            invoice.ReceivedDate = dto.ReceivedDate.Value;
            invoice.DueDate = dto.DueDate.Value;
            invoice.NetAmount = dto.NetAmount;
            invoice.VatAmount = dto.VatAmount;
            invoice.GrossAmount = dto.GrossAmount;
            invoice.Description = dto.Description;
            invoice.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        private static bool IsStatusTransitionAllowed(InvoiceStatus currentStatus, InvoiceStatus newStatus)
        {
            return currentStatus switch
            {
                InvoiceStatus.Draft => newStatus is InvoiceStatus.Submitted or InvoiceStatus.Cancelled,

                InvoiceStatus.Submitted => newStatus is InvoiceStatus.UnderReview or InvoiceStatus.Cancelled,

                InvoiceStatus.UnderReview => newStatus is InvoiceStatus.Approved
                    or InvoiceStatus.Rejected
                    or InvoiceStatus.Cancelled,

                InvoiceStatus.Approved => newStatus is InvoiceStatus.Booked or InvoiceStatus.Cancelled,

                InvoiceStatus.Booked => newStatus is InvoiceStatus.Paid or InvoiceStatus.Cancelled,

                InvoiceStatus.Rejected => newStatus is InvoiceStatus.Draft or InvoiceStatus.Cancelled,

                InvoiceStatus.Paid => false,

                InvoiceStatus.Cancelled => false,

                _ => false
            };
        }

        public async Task<bool> ChangeStatusAsync(
            int invoiceId,
            InvoiceStatus newStatus,
            string userId,
            string? description = null)
        {
            var invoice = await _dbContext.Invoices
                .FirstOrDefaultAsync(invoice => invoice.Id == invoiceId);

            if (invoice is null)
            {
                return false;
            }

            var oldStatus = invoice.Status;

            if (!IsStatusTransitionAllowed(oldStatus, newStatus))
            {
                throw new InvalidOperationException(
                    $"Nie można zmienić statusu z {oldStatus} na {newStatus}.");
            }

            invoice.Status = newStatus;
            invoice.UpdatedAt = DateTime.UtcNow;

            var auditLog = new InvoiceAuditLog
            {
                InvoiceId = invoice.Id,
                UserId = userId,
                Action = "StatusChanged",
                OldStatus = oldStatus,
                NewStatus = newStatus,
                Description = description ?? $"Zmieniono status z {oldStatus} na {newStatus}.",
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.InvoiceAuditLogs.Add(auditLog);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<InvoiceAuditLogDto>> GetInvoiceAuditLogsAsync(int invoiceId)
        {
            return await _dbContext.InvoiceAuditLogs
                .AsNoTracking()
                .Where(log => log.InvoiceId == invoiceId)
                .OrderByDescending(log => log.CreatedAt)
                .Select(log => new InvoiceAuditLogDto
                {
                    Id = log.Id,
                    UserEmail = log.User.Email ?? string.Empty,
                    Action = log.Action,
                    OldStatus = log.OldStatus,
                    NewStatus = log.NewStatus,
                    Description = log.Description,
                    CreatedAt = log.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<List<InvoiceCommentDto>> GetInvoiceCommentsAsync(int invoiceId)
        {
            return await _dbContext.InvoiceComments
                .AsNoTracking()
                .Where(comment => comment.InvoiceId == invoiceId)
                .OrderBy(comment => comment.CreatedAt)
                .Select(comment => new InvoiceCommentDto
                {
                    Id = comment.Id,
                    UserEmail = comment.User.Email ?? string.Empty,
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> AddInvoiceCommentAsync(int invoiceId, string userId, string content)
        {
            var invoiceExists = await _dbContext.Invoices
                .AnyAsync(invoice => invoice.Id == invoiceId);

            if (!invoiceExists)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new InvalidOperationException("Komentarz nie może być pusty.");
            }

            var invoiceComment = new InvoiceComment
            {
                InvoiceId = invoiceId,
                UserId = userId,
                Content = content.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.InvoiceComments.Add(invoiceComment);

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
