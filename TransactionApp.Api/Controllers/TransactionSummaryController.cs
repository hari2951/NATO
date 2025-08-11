using Microsoft.AspNetCore.Mvc;
using TransactionApp.Application.Interfaces;
using TransactionApp.Application.Utilities;

namespace TransactionApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionSummaryController(
        ITransactionSummaryService transactionSummaryService,
        ILogger<TransactionSummaryController> logger)
        : ControllerBase
    {
        [HttpGet("total-per-user")]
        public async Task<IActionResult> GetTotalsPerUser([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            logger.LogInformation(LogMessages.FetchingTotalTransactionsPerUser, pageNumber, pageSize);
            var result = await transactionSummaryService.GetTransactionsPerUserAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("total-per-type")]
        public async Task<IActionResult> GetTotalsPerType([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            logger.LogInformation(LogMessages.FetchingTotalsTransactionsPerType, pageNumber, pageSize);
            var result = await transactionSummaryService.GetTransactionsPerTypeAsync(pageNumber, pageSize);
            return Ok(result);
        }


        [HttpGet("high-volume")]
        public async Task<IActionResult> GetHighVolume([FromQuery] decimal threshold, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (threshold <= 0)
            {
                return BadRequest(LogMessages.ThresholdWarning);
            }

            logger.LogInformation(LogMessages.FetchingHigVolumeTransactions, threshold, pageNumber, pageSize);
            var result = await transactionSummaryService.GetHighVolumeTransactionsAsync(threshold, pageNumber, pageSize);
            return Ok(result);
        }
    }
}