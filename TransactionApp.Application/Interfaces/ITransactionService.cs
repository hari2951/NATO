using TransactionApp.Application.DTOs;
using TransactionApp.Application.Utilities;

namespace TransactionApp.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<PagedResult<TransactionDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<TransactionDto> GetByIdAsync(int id);
        Task<TransactionDto> CreateAsync(CreateTransactionDto dto);
    }
}