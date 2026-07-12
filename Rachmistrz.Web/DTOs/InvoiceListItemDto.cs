using Rachmistrz.Web.Enums;

namespace Rachmistrz.Web.DTOs
{
    public class InvoiceListItemDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string CostCategoryName { get; set; } = string.Empty;
        public decimal GrossAmount { get; set; }
        public DateTime DueDate { get; set; }
        public InvoiceStatus Status { get; set; }

    }
}
