using AutoMapper;
using Microsoft.Extensions.Logging;
using TransactionApp.Application.DTOs;
using TransactionApp.Application.Exceptions;
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
    IUserRepository userRepository,
    IMapper mapper,
    ILogger<TransactionService> logger,
    ICustomCache cache)
    : ITransactionService
    {
        public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
        {
            try
            {
                logger.LogInformation(LogMessages.CreatingTransaction, dto.UserId);

                var user = await userRepository.GetByIdAsync(dto.UserId);
                if (user == null)
                {
                    throw new NotFoundException(string.Format(LogMessages.UserNotPresent, dto.UserId));
                }

                var transaction = mapper.Map<Transaction>(dto);
                transaction.CreatedAt = DateTime.UtcNow;

                await repository.AddAsync(transaction);
                await repository.SaveAsync();

                ClearCache(transaction.UserId, transaction.TransactionType);

                logger.LogInformation(LogMessages.TransactionCreated, transaction.Id);

                return mapper.Map<TransactionDto>(transaction);
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, LogMessages.UserNotPresent, dto.UserId);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, LogMessages.FailedCreatingTransaction, dto.UserId);
                throw;
            }
        }

        public async Task<PagedResult<TransactionDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            try
            {
                logger.LogInformation(LogMessages.FetchingAllTransactionsDetails, pageNumber, pageSize);
                var (transactions, totalCount) = await repository.GetAllAsync(pageNumber, pageSize);
                var dtos = mapper.Map<IEnumerable<TransactionDto>>(transactions);

                return new PagedResult<TransactionDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, LogMessages.FailedFetchingTransactions);
                throw;
            }
        }

        public async Task<TransactionDto> GetByIdAsync(int id)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogError(ex, LogMessages.FailedFetchingTransaction, id);
                throw;
            }
        }

        private void ClearCache(string userId, TransactionTypeEnum transactionType)
        {
            var prefix = CacheKeyHelper.GetCachePrefix(userId);
            cache.RemoveByPrefix(prefix);
            logger.LogInformation(LogMessages.TransactionCacheCleared, prefix);
        }
    }
}