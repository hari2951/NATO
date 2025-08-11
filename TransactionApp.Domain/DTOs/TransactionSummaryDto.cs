using TransactionApp.Domain.Enums;

namespace TransactionApp.Application.DTOs
{
    public class TransactionSummaryDto
    {
        public string UserId { get; set; }
        public TransactionTypeEnum? TransactionType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
