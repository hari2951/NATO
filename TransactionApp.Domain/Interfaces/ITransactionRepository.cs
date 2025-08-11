using TransactionApp.Application.DTOs;
using TransactionApp.Domain.DTOs;
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
        Task<List<TransactionAmountRow>> GetAllTransactionsForSummaryAsync();
        Task<(IEnumerable<Transaction> Items, int TotalCount)> GetHighVolumeAsync(decimal threshold, int pageNumber, int pageSize);
    }
}
