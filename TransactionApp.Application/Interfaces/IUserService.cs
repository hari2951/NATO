using TransactionApp.Application.DTOs;

namespace TransactionApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(string id);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<bool> UpdateAsync(string id, CreateUserDto dto);
        Task<bool> DeleteAsync(string id);

    }
}