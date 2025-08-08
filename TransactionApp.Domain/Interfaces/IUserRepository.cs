using TransactionApp.Domain.Entities;

namespace TransactionApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<(IEnumerable<User> Users, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<User> GetByIdAsync(string id);
        Task AddAsync(User user);
        void Update(User user);
        void Delete(User user);
        Task SaveAsync();
    }
}
