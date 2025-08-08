namespace TransactionApp.Application.DTOs
{
    public class TransactionSummaryDto
    {
        public string UserId { get; set; }
        public string Type { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
