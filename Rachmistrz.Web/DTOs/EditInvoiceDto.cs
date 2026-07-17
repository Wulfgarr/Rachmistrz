using System.ComponentModel.DataAnnotations;

namespace Rachmistrz.Web.DTOs
{
    public class EditInvoiceDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Numer faktury jest wymagany.")]
        [StringLength(100, ErrorMessage = "Numer faktury może mieć maksymalnie 100 znaków.")]
        public string InvoiceNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Dostawca jest wymagany.")]
        public int? SupplierId { get; set; }
        [Required(ErrorMessage = "Oddział jest wymagany.")]
        public int? BranchId { get; set; }
        [Required(ErrorMessage = "Kategoria kosztów jest wymagana.")]
        public int? CostCategoryId { get; set; }
        [Required(ErrorMessage = "Data wystawienia jest wymagana.")]
        public DateTime? IssueDate { get; set; }
        [Required(ErrorMessage = "Data otrzymania jest wymagana.")]
        public DateTime? ReceivedDate { get; set; }
        [Required(ErrorMessage = "Termin płatności jest wymagany.")]
        public DateTime? DueDate { get; set; }
        [Range(0.01, 999999999, ErrorMessage = "Kwota netto musi być większa od 0.")]
        public decimal NetAmount { get; set; }
        [Range(0.01, 999999999, ErrorMessage = "Kwota VAT nie może być ujemna.")]
        public decimal VatAmount { get; set; }
        [Range(0.01, 999999999, ErrorMessage = "Kwota netto musi być większa od 0.")]
        public decimal GrossAmount { get; set; }
        [StringLength(1000, ErrorMessage = "Opis może mieć maksymalnie 1000 znaków.")]
        public string Description { get; set; }
    }
}
