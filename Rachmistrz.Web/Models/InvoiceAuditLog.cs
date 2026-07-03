using Rachmistrz.Web.Data;
using Rachmistrz.Web.Enums;

namespace Rachmistrz.Web.Models;

public class InvoiceAuditLog
{
    public int Id { get; set; }

    // Invoice PK
    public int InvoiceId { get; set; }

    public Invoice Invoice { get; set; } = null!;

    // User PK
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    // Name of action.
    public string Action { get; set; } = string.Empty;

    public InvoiceStatus? OldStatus { get; set; }

    public InvoiceStatus? NewStatus { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}