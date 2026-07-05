namespace Rachmistrz.Web.Models;

public class Supplier
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Nip { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    // Can not delete supplier, only activate/deactivate.
    public bool IsActive { get; set; } = true;

    public ICollection<Invoice> Invoices { get; set; }
    = new List<Invoice>();
}