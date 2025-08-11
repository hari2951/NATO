using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using TransactionApp.Application.DTOs;
using TransactionApp.Application.Services;
using TransactionApp.Domain.Entities;
using TransactionApp.Domain.Interfaces;

namespace TransactionApp.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repo = new();
        private readonly Mock<ILogger<UserService>> _logger = new();
        private readonly IMapper _mapper;

        public UserServiceTests()
        {
            var loggerFactory = LoggerFactory.Create(_ => { });
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();

                cfg.CreateMap<CreateUserDto, User>()
                    .ForMember(d => d.Id, opt => opt.Ignore())
                    .ForMember(d => d.Transactions, opt => opt.Ignore());
            }, loggerFactory);

            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsPagedResult()
        {
            // arrange
            var users = new List<User>
            {
                new() { Id = "user-1", FirstName = "Tom", LastName = "Jose", Email = "TomJose@test.com" },
                new() { Id = "user-2", FirstName = "Bilal", LastName = "Charlie", Email = "Bilal@test.com" }
            };
            _repo.Setup(r => r.GetAllAsync(1, 10))
                 .ReturnsAsync((users as IEnumerable<User>, users.Count));

            var userService = new UserService(_repo.Object, _mapper, _logger.Object);

            // act
            var page = await userService.GetAllAsync(1, 10);

            // assert
            page.TotalCount.Should().Be(2);
            page.Items.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_WhenNotFound_ReturnsNull()
        {
            // arrange
            _repo.Setup(r => r.GetByIdAsync("user123")).ReturnsAsync((User)null);
            var userService = new UserService(_repo.Object, _mapper, _logger.Object);

            // act
            var dto = await userService.GetByIdAsync("user123");

            // assert
            dto.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_Persists_And_ReturnsDto()
        {
            // arrange
            _repo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _repo.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

            var userService = new UserService(_repo.Object, _mapper, _logger.Object);

            // act
            var dto = await userService.CreateAsync(new CreateUserDto { FirstName = "Tom", LastName = "Jose", Email = "TomJose@test.com" });

            // assert
            dto.Should().NotBeNull();
            _repo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            _repo.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenMissing_ReturnsFalse()
        {
            // arrange
            _repo.Setup(r => r.GetByIdAsync("u999")).ReturnsAsync((User)null);
            var userService = new UserService(_repo.Object, _mapper, _logger.Object);

            // act
            var ok = await userService.UpdateAsync("u999", new CreateUserDto { FirstName = "Tom", LastName = "Jose", Email = "TomJose@test.com" });

            // assert
            ok.Should().BeFalse();
            _repo.Verify(r => r.SaveAsync(), Times.Never);
        }
    }
}
