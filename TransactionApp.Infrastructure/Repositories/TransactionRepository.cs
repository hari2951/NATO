using Microsoft.EntityFrameworkCore;
using TransactionApp.Domain.Entities;
using TransactionApp.Domain.Enums;
using TransactionApp.Domain.Interfaces;
using TransactionApp.Infrastructure.Data;

namespace TransactionApp.Infrastructure.Repositories
{
    public class TransactionRepository(AppDbContext context) : ITransactionRepository
    {
        public async Task<(IEnumerable<Transaction> Transactions, int TotalCount)> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = context.Transactions.Include(t => t.User).AsQueryable();

            var totalCount = await query.CountAsync();
            var transactions = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (transactions, totalCount);
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            return await context.Transactions.FindAsync(id);
        }

        public async Task AddAsync(Transaction transaction)
        {
            await context.Transactions.AddAsync(transaction);
        }


        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
        public async Task<decimal> GetTotalAmountByUserAndTypeAsync(string userId, TransactionTypeEnum? transactionType, DateTime? startDate, DateTime? endDate)
        {
            var query = context.Transactions
                .AsNoTracking()
                .Where(t => t.UserId == userId);
            
            if (transactionType.HasValue)
                query = query.Where(t => t.TransactionType == transactionType);

            if (startDate.HasValue)
                query = query.Where(t => t.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.CreatedAt <= endDate.Value);

            return await query.SumAsync(t => t.Amount);
        }
    }
}