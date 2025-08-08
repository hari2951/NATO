using TransactionApp.Domain.Entities;
using TransactionApp.Domain.Enums;

namespace TransactionApp.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction> GetByIdAsync(int id);
        Task AddAsync(Transaction transaction);
        Task SaveAsync();
        Task<decimal> GetTotalAmountByUserAndTypeAsync(string userId, TransactionTypeEnum? type, DateTime? startDate, DateTime? endDate);
    }
}
