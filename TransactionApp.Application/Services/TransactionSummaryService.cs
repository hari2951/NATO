using AutoMapper;
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
        ILogger<TransactionSummaryService> logger,
        IMapper mapper)
        : ITransactionSummaryService
    {
        private readonly int _cacheDurationMinutes = cacheSettings.Value.TransactionSummaryCacheDurationMinutes;
        private const string CacheKey = Constants.AllTransactionsCacheKey;

        public async Task<PagedResult<UserTotalDto>> GetTransactionsPerUserAsync(int pageNumber, int pageSize)
        {
            if (!cache.TryGetValue(CacheKey, out List<UserTotalDto> allTransactions))
            {
                logger.LogInformation(LogMessages.CacheMiss, CacheKey);

                var transactionRows = await repository.GetAllTransactionsForSummaryAsync();

                var transactionDict = new Dictionary<string, (decimal total, string firstName, string lastName)>(StringComparer.Ordinal);
                foreach (var row in transactionRows)
                {
                    if (transactionDict.TryGetValue(row.UserId, out var agg))
                        transactionDict[row.UserId] = (agg.total + row.Amount, agg.firstName, agg.lastName);
                    else
                        transactionDict[row.UserId] = (row.Amount, row.FirstName, row.LastName);
                }

                allTransactions = transactionDict.Select(kv => new UserTotalDto
                {
                    UserId = kv.Key,
                    FullName = $"{kv.Value.firstName} {kv.Value.lastName}".Trim(),
                    TotalAmount = kv.Value.total
                })
                        .OrderByDescending(x => x.TotalAmount)
                        .ToList();

                cache.Set(CacheKey, allTransactions, TimeSpan.FromMinutes(_cacheDurationMinutes));
            }
            else
            {
                logger.LogInformation(LogMessages.CacheHit, CacheKey);
            }

            var totalCount = allTransactions.Count;
            var items = allTransactions.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<UserTotalDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<TransactionTypeTotalDto>> GetTransactionsPerTypeAsync(int pageNumber, int pageSize)
        {
            if (!cache.TryGetValue(CacheKey, out List<TransactionTypeTotalDto> allTransactions))
            {
                logger.LogInformation(LogMessages.CacheMiss, CacheKey);
                var transactionRows = await repository.GetAllTransactionsForSummaryAsync();

                var transactionDict = new Dictionary<TransactionTypeEnum, decimal>();
                foreach (var row in transactionRows)
                {
                    if (transactionDict.TryGetValue(row.TransactionType, out var total))
                        transactionDict[row.TransactionType] = total + row.Amount;
                    else
                        transactionDict[row.TransactionType] = row.Amount;
                }

                allTransactions = transactionDict.Select(kv => new TransactionTypeTotalDto
                {
                    TransactionType = kv.Key,
                    TotalAmount = kv.Value
                })
                        .OrderByDescending(x => x.TotalAmount)
                        .ToList();

                cache.Set(CacheKey, allTransactions, TimeSpan.FromMinutes(_cacheDurationMinutes));
            }
            else
            {
                logger.LogInformation(LogMessages.CacheHit, CacheKey);
            }

            var totalCount = allTransactions.Count;
            var items = allTransactions.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<TransactionTypeTotalDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<TransactionDto>> GetHighVolumeTransactionsAsync(decimal threshold, int pageNumber, int pageSize)
        {
            logger.LogInformation(LogMessages.FetchingHighVolume, threshold);

            var (entities, totalCount) = await repository.GetHighVolumeAsync(threshold, pageNumber, pageSize);
            var transactionDtos = mapper.Map<IEnumerable<TransactionDto>>(entities);

            return new PagedResult<TransactionDto>
            {
                Items = transactionDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}