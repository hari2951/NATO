using TransactionApp.Domain.Enums;

namespace TransactionApp.Application.Utilities.Caching
{
    public static class CacheKeyHelper
    {
        public static string GetCachePrefix(string userId, TransactionTypeEnum type)
        {
            return $"TransactionSummary_{userId}_{type}";
        }

        public static string GetCacheKey(string userId, TransactionTypeEnum type, DateTime? startDate, DateTime? endDate)
        {
            var key = GetCachePrefix(userId, type);

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