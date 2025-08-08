using TransactionApp.Domain.Entities;
using TransactionApp.Domain.Enums;

namespace TransactionApp.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<(IEnumerable<Transaction> Transactions, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<Transaction> GetByIdAsync(int id);
        Task AddAsync(Transaction transaction);
        Task SaveAsync();
        Task<decimal> GetTotalAmountByUserAndTypeAsync(string userId, TransactionTypeEnum? transactionType, DateTime? startDate, DateTime? endDate);
    }
}
