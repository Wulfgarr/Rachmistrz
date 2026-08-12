namespace Rachmistrz.Web.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalInvoicesCount { get; set; }

        public int DraftInvoicesCount { get; set; }

        public int SubmittedInvoicesCount { get; set; }

        public int UnderReviewInvoicesCount { get; set; }

        public int ApprovedInvoicesCount { get; set; }

        public int BookedInvoicesCount { get; set; }

        public int PaidInvoicesCount { get; set; }

        public int OverdueInvoicesCount { get; set; }

        public decimal CurrentMonthGrossAmount { get; set; }
    }
}
