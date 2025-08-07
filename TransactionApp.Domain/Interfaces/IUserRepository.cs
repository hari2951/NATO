using TransactionApp.Domain.Entities;

namespace TransactionApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task AddAsync(User user);
        void Update(User user);
        void Delete(User user);
        Task SaveAsync();
    }
}
