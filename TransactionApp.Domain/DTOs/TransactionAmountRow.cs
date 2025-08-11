using TransactionApp.Domain.Enums;

namespace TransactionApp.Domain.DTOs
{
    public class TransactionAmountRow
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}
