using Microsoft.EntityFrameworkCore;
using TransactionApp.Domain.Entities;
using TransactionApp.Domain.Interfaces;
using TransactionApp.Infrastructure.Data;

namespace TransactionApp.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<(IEnumerable<User> Users, int TotalCount)> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = context.Users.AsQueryable();

            var totalCount = await query.CountAsync();
            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, totalCount);
        }

        public async Task<User> GetByIdAsync(string id)
            => await context.Users.FindAsync(id);

        public async Task AddAsync(User user)
            => await context.Users.AddAsync(user);

        public void Update(User user)
            => context.Users.Update(user);

        public void Delete(User user)
            => context.Users.Remove(user);

        public async Task SaveAsync()
            => await context.SaveChangesAsync();
    }
}
