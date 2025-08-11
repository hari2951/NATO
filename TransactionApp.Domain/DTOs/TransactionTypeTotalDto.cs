using TransactionApp.Domain.Enums;

namespace TransactionApp.Application.DTOs
{
    public class TransactionTypeTotalDto
    {
        public TransactionTypeEnum TransactionType { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
