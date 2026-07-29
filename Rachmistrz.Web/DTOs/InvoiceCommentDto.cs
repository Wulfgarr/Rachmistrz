namespace Rachmistrz.Web.DTOs
{
    public class InvoiceCommentDto
    {
        public int Id { get; set; }

        public string UserEmail { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
