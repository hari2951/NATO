using Microsoft.EntityFrameworkCore;
using TransactionApp.Domain.Entities;
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
    }
}