using Rachmistrz.Web.Enums;

namespace Rachmistrz.Web.DTOs
{
    public class InvoiceAuditLogDto
    {
        public int Id { get; set; }

        public string UserEmail { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public InvoiceStatus? OldStatus { get; set; }

        public InvoiceStatus? NewStatus { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
