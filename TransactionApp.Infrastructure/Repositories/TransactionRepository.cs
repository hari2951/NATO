using Microsoft.EntityFrameworkCore;
using TransactionApp.Domain.Entities;
using TransactionApp.Domain.Enums;
using TransactionApp.Domain.Interfaces;
using TransactionApp.Infrastructure.Data;

namespace TransactionApp.Infrastructure.Repositories
{
    public class TransactionRepository(AppDbContext context) : ITransactionRepository
    {
        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await context.Transactions.ToListAsync();
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
        public async Task<decimal> GetTotalAmountByUserAndTypeAsync(string userId, TransactionTypeEnum? type, DateTime? startDate, DateTime? endDate)
        {
            var query = context.Transactions
                .AsNoTracking()
                .Where(t => t.UserId == userId);
            
            if (type.HasValue)
                query = query.Where(t => t.TransactionType == type);

            if (startDate.HasValue)
                query = query.Where(t => t.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.CreatedAt <= endDate.Value);

            return await query.SumAsync(t => t.Amount);
        }
    }
}