using TransactionApp.Domain.Enums;

namespace TransactionApp.Application.Utilities.Caching
{
    public static class CacheKeyHelper
    {
        public static string GetCachePrefix(string userId)
        {
            return $"TransactionSummary_{userId}";
        }

        public static string GetCacheKey(string userId, TransactionTypeEnum? transactionType, DateTime? startDate, DateTime? endDate)
        {
            var key = GetCachePrefix(userId);
            if (transactionType.HasValue)
            {
                key += $"_{transactionType}";
            }

            if (startDate.HasValue)
            {
                key += $"_S_{startDate.Value:yyyyMMdd}";
            }

            if (endDate.HasValue)
            {
                key += $"_E_{endDate.Value:yyyyMMdd}";
            }

            return key;
        }
    }
}