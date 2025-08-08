using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TransactionApp.Application.Configuration;
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
        IOptions<CacheSettings> cacheSettings,
        ILogger<TransactionSummaryService> logger)
        : ITransactionSummaryService
    {
        private readonly int _cacheDurationMinutes = cacheSettings.Value.TransactionSummaryCacheDurationMinutes;
        public async Task<TransactionSummaryDto> GetSummaryByUserAndTypeAsync(string userId, TransactionTypeEnum? transactionType, DateTime? startDate, DateTime? endDate)
        {
            var cacheKey = CacheKeyHelper.GetCacheKey(userId, transactionType, startDate, endDate);

            if (cache.TryGetValue(cacheKey, out TransactionSummaryDto cached))
            {
                logger.LogInformation(LogMessages.FetchingTransactionSummaryFromCache, cacheKey);
                return cached;
            }

            try
            {
                logger.LogInformation(LogMessages.FetchingTransactionsSummaryFromDb);

                var total = await repository.GetTotalAmountByUserAndTypeAsync(userId, transactionType, startDate, endDate);

                var summary = new TransactionSummaryDto
                {
                    UserId = userId,
                    TransactionType = transactionType,
                    TotalAmount = total,
                    StartDate = startDate,
                    EndDate = endDate
                };

                cache.Set(cacheKey, summary, TimeSpan.FromMinutes(_cacheDurationMinutes));
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