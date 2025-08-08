using TransactionApp.Application.DTOs;
using TransactionApp.Application.Utilities;

namespace TransactionApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<UserDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<UserDto> GetByIdAsync(string id);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<bool> UpdateAsync(string id, CreateUserDto dto);
        Task<bool> DeleteAsync(string id);
    }
}