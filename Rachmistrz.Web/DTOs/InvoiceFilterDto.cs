using Rachmistrz.Web.Enums;

namespace Rachmistrz.Web.DTOs
{
    public class InvoiceFilterDto
    {
        public string? InvoiceNumber { get; set; }
        public InvoiceStatus? Status { get; set; }
        public int? SupplierId { get; set; }
        public int? BranchId { get; set; }
        public DateTime? DueDateForm { get; set; }
        public DateTime? DueDateTo { get; set; }
    }
}
