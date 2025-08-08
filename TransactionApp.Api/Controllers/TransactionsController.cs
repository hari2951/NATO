using Microsoft.AspNetCore.Mvc;
using TransactionApp.Application.DTOs;
using TransactionApp.Application.Interfaces;
using TransactionApp.Application.Utilities;

namespace TransactionApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController(ITransactionService service, ILogger<TransactionsController> logger)
        : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
        {
            logger.LogInformation(LogMessages.CreatingTransaction, dto.UserId);

            var created = await service.CreateAsync(dto);

            logger.LogInformation(LogMessages.TransactionCreated, created.Id);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            logger.LogInformation(LogMessages.FetchingAllTransactions);

            var pagedResult = await service.GetAllAsync(pageNumber, pageSize);
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            logger.LogInformation(LogMessages.FetchingTransaction, id);

            var transaction = await service.GetByIdAsync(id);
            if (transaction != null)
            {
                return Ok(transaction);
            }

            logger.LogWarning(LogMessages.TransactionNotFound, id);
            
            return NotFound();
        }
    }
}
