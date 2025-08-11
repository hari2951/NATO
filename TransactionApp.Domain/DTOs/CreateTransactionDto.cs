using TransactionApp.Domain.Enums;

namespace TransactionApp.Application.DTOs
{
    public class CreateTransactionDto
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
    }
}
