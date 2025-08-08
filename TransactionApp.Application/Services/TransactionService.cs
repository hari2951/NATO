using AutoMapper;
using Microsoft.Extensions.Logging;
using TransactionApp.Application.DTOs;
using TransactionApp.Application.Interfaces;
using TransactionApp.Application.Utilities;
using TransactionApp.Application.Utilities.Caching;
using TransactionApp.Domain.Entities;
using TransactionApp.Domain.Enums;
using TransactionApp.Domain.Interfaces;

namespace TransactionApp.Application.Services
{
    public class TransactionService(
        ITransactionRepository repository,
        IMapper mapper,
        ILogger<TransactionService> logger,
        ICustomCache cache)
        : ITransactionService
    {
        public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
        {
            logger.LogInformation(LogMessages.CreatingTransaction, dto.UserId);

            var transaction = mapper.Map<Transaction>(dto);
            transaction.CreatedAt = DateTime.UtcNow;

            await repository.AddAsync(transaction);
            await repository.SaveAsync();

            ClearCache(transaction.UserId, transaction.TransactionType);

            logger.LogInformation(LogMessages.TransactionCreated, transaction.Id);

            return mapper.Map<TransactionDto>(transaction);
        }

        public async Task<IEnumerable<TransactionDto>> GetAllAsync() 
            => mapper.Map<IEnumerable<TransactionDto>>(await repository.GetAllAsync());

        public async Task<TransactionDto> GetByIdAsync(int id)
        {
            logger.LogInformation(LogMessages.FetchingTransaction, id);

            var transaction = await repository.GetByIdAsync(id);
            if (transaction is not null)
            {
                return mapper.Map<TransactionDto>(transaction);
            }

            logger.LogWarning(LogMessages.TransactionNotFound, id);
            
            return null;
        }

        private void ClearCache(string userId, TransactionTypeEnum transactionType)
        {
            var prefix = CacheKeyHelper.GetCachePrefix(userId, transactionType);
            cache.RemoveByPrefix(prefix);
            logger.LogInformation("Cleared cache with prefix: {CachePrefix}", prefix);
        }
    }
}