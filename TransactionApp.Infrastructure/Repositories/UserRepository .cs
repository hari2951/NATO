using Microsoft.EntityFrameworkCore;
using TransactionApp.Domain.Entities;
using TransactionApp.Domain.Interfaces;
using TransactionApp.Infrastructure.Data;

namespace TransactionApp.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<IEnumerable<User>> GetAllAsync()
            => await context.Users.ToListAsync();

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
