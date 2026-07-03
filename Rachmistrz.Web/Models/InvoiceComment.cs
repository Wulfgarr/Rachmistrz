using Rachmistrz.Web.Data;

namespace Rachmistrz.Web.Models;

public class InvoiceComment
{
    public int Id { get; set; }

    // Commented invoice id.
    public int InvoiceId { get; set; }

    public Invoice Invoice { get; set; } = null!;

    // Comment author.
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}