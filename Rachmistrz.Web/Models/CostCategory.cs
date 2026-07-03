namespace Rachmistrz.Web.Models;

public class CostCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // Can not delete, only activate/deactivate
    public bool IsActive { get; set; } = true;

    public ICollection<Invoice> Invoices { get; set; }
    = new List<Invoice>();
}