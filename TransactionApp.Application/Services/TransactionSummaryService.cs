using Microsoft.Extensions.Logging;
using TransactionApp.Application.DTOs;
using TransactionApp.Application.Interfaces;
using TransactionApp.Application.Utilities;
using TransactionApp.Application.Utilities.Caching;
using TransactionApp.Domain.Enums;
using TransactionApp.Domain.Interfaces;

namespace TransactionApp.Application.Services
{
    public class TransactionSummaryService(
        ITransactionRepository repository,
        ICustomCache cache,
        ILogger<TransactionSummaryService> logger)
        : ITransactionSummaryService
    {
        public async Task<TransactionSummaryDto> GetSummaryByUserAndTypeAsync(
            string userId,
            TransactionTypeEnum? type,
            DateTime? startDate,
            DateTime? endDate)
        {
            var cacheKey = CacheKeyHelper.GetCacheKey(userId, type, startDate, endDate);

            if (cache.TryGetValue(cacheKey, out TransactionSummaryDto cached))
            {
                logger.LogInformation(LogMessages.FetchingTransactionSummaryFromCache, cacheKey);
                return cached;
            }

            try
            {
                logger.LogInformation(LogMessages.FetchingTransactionsSummaryFromDb);

                var total = await repository.GetTotalAmountByUserAndTypeAsync(userId, type, startDate, endDate);

                var summary = new TransactionSummaryDto
                {
                    UserId = userId,
                    Type = type,
                    TotalAmount = total,
                    StartDate = startDate,
                    EndDate = endDate
                };

                cache.Set(cacheKey, summary, TimeSpan.FromMinutes(10));
                return summary;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, LogMessages.FetchingTransactionSummaryError);
                throw;
            }
        }
    }
}