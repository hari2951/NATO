using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TransactionApp.Application.DTOs;
using TransactionApp.Application.Exceptions;
using TransactionApp.Application.Services;
using TransactionApp.Application.Utilities.Caching;
using TransactionApp.Domain.Entities;
using TransactionApp.Domain.Enums;
using TransactionApp.Domain.Interfaces;

namespace TransactionApp.UnitTests.Services
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _txRepo = new();
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<ICustomCache> _cache = new();
        private readonly Mock<ILogger<TransactionService>> _logger = new();
        private readonly IMapper _mapper;

        public TransactionServiceTests()
        {
            Mock<ITransactionRepository> transactionRepoMock = new();
            Mock<IUserRepository> userRepoMock = new();
            Mock<ICustomCache> cacheMock = new();

            var loggerFactory = LoggerFactory.Create(_ => { });


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateTransactionDto, Transaction>()
                    .ForMember(d => d.Id, o => o.Ignore())
                    .ForMember(d => d.User, o => o.Ignore())
                    .ForMember(d => d.CreatedAt, o => o.Ignore());

                cfg.CreateMap<Transaction, TransactionDto>()
                    .ForMember(d => d.FullName,
                        o => o.MapFrom(s => s.User != null
                            ? $"{s.User.FirstName} {s.User.LastName}".Trim()
                            : null));
            }, loggerFactory);

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task CreateAsync_WhenUserMissing_ThrowsNotFound()
        {
            _userRepo.Setup(r => r.GetByIdAsync("")).ReturnsAsync((User)null);

            var transactionService = new TransactionService(_txRepo.Object, _userRepo.Object, _mapper, _logger.Object, _cache.Object);

            var act = () => transactionService.CreateAsync(new CreateTransactionDto
            {
                UserId = "",
                Amount = 10,
                TransactionType = TransactionTypeEnum.Debit
            });

            await act.Should().ThrowAsync<NotFoundException>();
            _txRepo.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_Valid_Persists_And_ClearsCache()
        {
            _userRepo.Setup(r => r.GetByIdAsync("user2255")).ReturnsAsync(new User { Id = "user2255" });
            _txRepo.Setup(r => r.AddAsync(It.IsAny<Transaction>())).Returns(Task.CompletedTask);
            _txRepo.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

            var transactionService = new TransactionService(_txRepo.Object, _userRepo.Object, _mapper, _logger.Object, _cache.Object);

            var dto = await transactionService.CreateAsync(new CreateTransactionDto
            {
                UserId = "user2255",
                Amount = 99,
                TransactionType = TransactionTypeEnum.Credit
            });

            dto.Should().NotBeNull();
            _txRepo.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Once);
            _txRepo.Verify(r => r.SaveAsync(), Times.Once);
            _cache.Verify(c => c.RemoveByKey(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsPaged()
        {
            var list = new List<Transaction> { new() { Id = 1, UserId = "user2255", Amount = 1, TransactionType = TransactionTypeEnum.Debit } };
            _txRepo.Setup(r => r.GetAllAsync(1, 10)).ReturnsAsync((list as IEnumerable<Transaction>, 1));

            var transactionService = new TransactionService(_txRepo.Object, _userRepo.Object, _mapper, _logger.Object, _cache.Object);

            var page = await transactionService.GetAllAsync(1, 10);

            page.TotalCount.Should().Be(1);
            page.Items.Should().HaveCount(1);
        }
    }
}