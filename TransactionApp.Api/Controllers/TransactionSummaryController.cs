using Microsoft.AspNetCore.Mvc;
using TransactionApp.Application.Interfaces;
using TransactionApp.Domain.Enums;

namespace TransactionApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionSummaryController(
        ITransactionSummaryService transactionSummaryService,
        ILogger<TransactionSummaryController> logger)
        : ControllerBase
    {
        [HttpGet("by-user-and-type")]
        public async Task<IActionResult> GetByUserAndType(
            [FromQuery] string userId,
            [FromQuery] TransactionTypeEnum? transactionType,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                logger.LogWarning("Missing required parameter: userId");
                return BadRequest("Parameter 'userId' is required.");
            }

            var result = await transactionSummaryService.GetSummaryByUserAndTypeAsync(
                userId, transactionType, startDate, endDate);

            return Ok(result);
        }
    }
}