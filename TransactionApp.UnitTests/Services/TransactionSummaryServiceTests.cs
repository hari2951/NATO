using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using FluentAssertions;
using TransactionApp.Application.Configuration;
using TransactionApp.Application.DTOs;
using TransactionApp.Application.Services;
using TransactionApp.Application.Utilities.Caching;
using TransactionApp.Domain.DTOs;
using TransactionApp.Domain.Enums;
using TransactionApp.Domain.Interfaces;

namespace TransactionApp.UnitTests.Services
{
    public class TransactionSummaryServiceTests
    {
        private readonly Mock<ITransactionRepository> _repo = new();
        private readonly Mock<ICustomCache> _cache = new();
        private readonly Mock<ILogger<TransactionSummaryService>> _logger = new();
        private readonly IMapper _mapper;
        private readonly IOptions<CacheSettings> _opts =
            Options.Create(new CacheSettings { TransactionSummaryCacheDurationMinutes = 10 });

        public TransactionSummaryServiceTests()
        {
            var loggerFactory = LoggerFactory.Create(_ => { });

            var config = new MapperConfiguration(cfg => {}, loggerFactory);

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetTransactionsPerUser_Computes_Totals_And_Paginates()
        {
            _cache.Setup(c => c.TryGetValue(It.IsAny<string>(), out It.Ref<List<UserTotalDto>>.IsAny))
                  .Returns(false);

            _repo.Setup(r => r.GetAllTransactionsForSummaryAsync())
                 .ReturnsAsync(new List<TransactionAmountRow>
                 {
                     new() { UserId = "u1", FirstName="A", LastName="A", TransactionType = TransactionTypeEnum.Debit, Amount = 100 },
                     new() { UserId = "u1", FirstName="A", LastName="A", TransactionType = TransactionTypeEnum.Credit, Amount = 50 },
                     new() { UserId = "u2", FirstName="B", LastName="B", TransactionType = TransactionTypeEnum.Credit, Amount = 10 }
                 });

            var transactionSummaryService = new TransactionSummaryService(_repo.Object, _cache.Object, _opts, _logger.Object, _mapper);

            var page = await transactionSummaryService.GetTransactionsPerUserAsync(1, 10);

            page.TotalCount.Should().Be(2);
            page.Items.Should().ContainSingle(x => x.UserId == "u1" && x.TotalAmount == 150);
            page.Items.Should().ContainSingle(x => x.UserId == "u2" && x.TotalAmount == 10);

            _cache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<List<UserTotalDto>>(), It.IsAny<TimeSpan>()), Times.Once);
        }

        [Fact]
        public async Task GetTransactionsPerType_UsesCache_WhenPresent()
        {
            var cached = new List<TransactionTypeTotalDto>
            {
                new() { TransactionType = TransactionTypeEnum.Debit, TotalAmount = 111 }
            };

            // emulate cache hit
            _cache.Setup(c => c.TryGetValue(It.IsAny<string>(), out cached)).Returns(true);

            var transactionSummaryService = new TransactionSummaryService(_repo.Object, _cache.Object, _opts, _logger.Object, _mapper);

            var page = await transactionSummaryService.GetTransactionsPerTypeAsync(1, 10);

            page.Items.Should().HaveCount(1);
            _repo.Verify(r => r.GetAllTransactionsForSummaryAsync(), Times.Never);
        }
    }
}
