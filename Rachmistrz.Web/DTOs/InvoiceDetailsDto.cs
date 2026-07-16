using Rachmistrz.Web.Enums;

namespace Rachmistrz.Web.DTOs
{
    public class InvoiceDetailsDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierNip { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string BranchCode { get; set; } = string.Empty;
        public string CostCategoryName { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public InvoiceStatus Status;
        public string Description { get; set; } = string.Empty;
        public string CreatedByUserEmail { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


    }
}
