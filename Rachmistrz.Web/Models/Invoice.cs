using Rachmistrz.Web.Data;
using Rachmistrz.Web.Enums;

namespace Rachmistrz.Web.Models;

public class Invoice
{
    public int Id { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;

    // Supplier FK
    public int SupplierId { get; set; }

    public Supplier Supplier { get; set; } = null!;

    // Branch FK
    public int BranchId { get; set; }

    public Branch Branch { get; set; } = null!;

    // CostCategory FK
    public int CostCategoryId { get; set; }

    public CostCategory CostCategory { get; set; } = null!;

    public DateTime IssueDate { get; set; }

    public DateTime ReceivedDate { get; set; }

    public DateTime DueDate { get; set; }

    public decimal NetAmount { get; set; }

    public decimal VatAmount { get; set; }

    public decimal GrossAmount { get; set; }

    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

    public string Description { get; set; } = string.Empty;

    // Identity using string as type of id by default 
    public string CreatedByUserId { get; set; } = string.Empty;

    public ApplicationUser CreatedByUser { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}