using Rachmistrz.Web.Enums;

namespace Rachmistrz.Web.DTOs
{
    public class MonthlyReportInvoiceDto
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public string SupplierName { get; set; } = string.Empty;

        public string BranchName { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public decimal NetAmount { get; set; }

        public decimal VatAmount { get; set; }

        public decimal GrossAmount { get; set; }

        public InvoiceStatus Status { get; set; }
    }
}
