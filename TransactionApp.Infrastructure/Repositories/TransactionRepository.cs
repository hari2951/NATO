using Microsoft.EntityFrameworkCore;
using TransactionApp.Domain.DTOs;
using TransactionApp.Domain.Entities;
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
            => await context.Transactions.FindAsync(id);

        public async Task AddAsync(Transaction transaction)
            => await context.Transactions.AddAsync(transaction);


        public async Task SaveAsync()
            => await context.SaveChangesAsync();

        public async Task<List<TransactionAmountRow>> GetAllTransactionsForSummaryAsync()
        {
            return await context.Transactions
                .AsNoTracking()
                .Include(t => t.User)
                .Select(t => new TransactionAmountRow
                {
                    UserId = t.UserId,
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName,
                    TransactionType = t.TransactionType,
                    Amount = t.Amount
                })
                .ToListAsync();
        }
     
        public async Task<(IEnumerable<Transaction> Items, int TotalCount)> GetHighVolumeAsync(decimal threshold, int pageNumber, int pageSize)
        {
            var query = context.Transactions
                .Where(t => t.Amount > threshold)
                .OrderByDescending(t => t.Amount);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}