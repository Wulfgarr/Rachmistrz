using System.ComponentModel.DataAnnotations;

namespace Rachmistrz.Web.DTOs
{
    public class CreateInvoiceDto
    {
        [Required(ErrorMessage = "Numer faktury jest wymagany.")]
        [StringLength(100, ErrorMessage = "Numer faktury może mieć maksymalnie 100 znaków.")]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Dostawca jest wymagany.")]
        public int? SupplierId { get; set; }

        [Required(ErrorMessage = "Oddział jest wymagany.")]
        public int? BranchId { get; set; }

        [Required(ErrorMessage = "Kategoria kosztów jet wymagana.")]
        public int? CostCategoryId { get; set; }

        [Required(ErrorMessage = "Data wystawienia jest wymagana.")]
        public DateTime? IssueDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Data otrzymania jest wymagana.")]
        public DateTime? ReceivedDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Termin płatności jest wymagany.")]
        public DateTime? DueDate { get; set; } = DateTime.Today.AddDays(14);

        [Required(ErrorMessage = "Kwota netto jest wymagana.")]
        [Range(0.01, 999999999, ErrorMessage = "Kwota netto musi być większa od 0.")]
        public decimal? NetAmount { get; set; }

        [Required(ErrorMessage = "Kwota VAT jest wymagana.")]
        [Range(0, 999999999, ErrorMessage = "Kwota Vat nie może być ujemna.")]
        public decimal? VatAmount { get; set; }

        [Required(ErrorMessage = "Kwota brutto jest wymagana.")]
        [Range(0.01, 999999999, ErrorMessage = "Kwota brutto musi być większa od 0.")]
        public decimal? GrossAmount { get; set; }

        [StringLength(1000, ErrorMessage = "Opis może mieć maksymalnie 1000znaków")]
        public string Description { get; set; } = string.Empty;
    }
}
