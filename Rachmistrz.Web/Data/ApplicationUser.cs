using Microsoft.AspNetCore.Identity;
using Rachmistrz.Web.Models;

namespace Rachmistrz.Web.Data
{
    
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<Invoice> CreatedInvoices { get; set; }
    = new List<Invoice>();

        public ICollection<InvoiceComment> InvoiceComments { get; set; }
            = new List<InvoiceComment>();

        public ICollection<InvoiceAuditLog> InvoiceAuditLogs { get; set; }
            = new List<InvoiceAuditLog>();

    }

}
