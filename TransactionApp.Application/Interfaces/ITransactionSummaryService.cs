using TransactionApp.Application.DTOs;
using TransactionApp.Application.Utilities;

namespace TransactionApp.Application.Interfaces
{
    public interface ITransactionSummaryService
    {
        Task<PagedResult<UserTotalDto>> GetTransactionsPerUserAsync(int pageNumber, int pageSize);
        Task<PagedResult<TransactionTypeTotalDto>> GetTransactionsPerTypeAsync(int pageNumber, int pageSize);
        Task<PagedResult<TransactionDto>> GetHighVolumeTransactionsAsync(decimal threshold, int pageNumber, int pageSize);
    }
}