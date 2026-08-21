namespace Rachmistrz.Web.DTOs
{
    public class MonthlyReportDto
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int InvoicesCount { get; set; }

        public decimal TotalNetAmount { get; set; }

        public decimal TotalVatAmount { get; set; }

        public decimal TotalGrossAmount { get; set; }

        public int PaidInvoicesCount { get; set; }

        public int UnpaidInvoicesCount { get; set; }

        public int OverdueInvoicesCount { get; set; }

        public List<MonthlyReportInvoiceDto> Invoices { get; set; } = [];
    }
}
