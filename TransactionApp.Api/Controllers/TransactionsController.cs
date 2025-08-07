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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await service.GetAllAsync();
            return Ok(transactions);
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
        {
            logger.LogInformation(LogMessages.CreatingTransaction, dto.UserId);

            var created = await service.CreateAsync(dto);

            logger.LogInformation(LogMessages.TransactionCreated, created.Id);
            
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}
