using TransactionApp.Application.DTOs;
using TransactionApp.Domain.Enums;

namespace TransactionApp.Application.Interfaces
{
    public interface ITransactionSummaryService
    {
        Task<TransactionSummaryDto> GetSummaryByUserAndTypeAsync(string userId, TransactionTypeEnum type, DateTime? startDate, DateTime? endDate);
    }
}