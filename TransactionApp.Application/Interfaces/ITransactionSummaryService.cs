using TransactionApp.Application.DTOs;
using TransactionApp.Domain.Enums;

namespace TransactionApp.Application.Interfaces
{
    public interface ITransactionSummaryService
    {
        Task<TransactionSummaryDto> GetSummaryByUserAndTypeAsync(string userId, TransactionTypeEnum? transactionType, DateTime? startDate, DateTime? endDate);
    }
}