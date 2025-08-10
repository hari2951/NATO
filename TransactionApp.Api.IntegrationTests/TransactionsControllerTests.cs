using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using TransactionApp.Api.IntegrationTests.TestHost;
using TransactionApp.Application.DTOs;
using TransactionApp.Application.Utilities;
using TransactionApp.Domain.Enums;

namespace TransactionApp.Api.IntegrationTests
{
    public class TransactionsControllerTests(CustomWebApplicationFactory factory)
        : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        [Fact]
        public async Task Create_SuccessfulTransaction_Returns201_WithBody()
        {
            var dto = new CreateTransactionDto
            {
                UserId = "user-1",
                Amount = 42.75m,
                TransactionType = TransactionTypeEnum.Credit
            };

            var resp = await _client.PostAsJsonAsync("/api/transactions", dto);

            resp.StatusCode.Should().Be(HttpStatusCode.Created);
            var body = await resp.Content.ReadFromJsonAsync<TransactionDto>();
            body.Should().NotBeNull();
            body!.UserId.Should().Be(dto.UserId);
            body.Amount.Should().Be(dto.Amount);
        }

        [Fact]
        public async Task Create_UserNotPresent_Returns404_WithErrorMessage()
        {
            var dto = new CreateTransactionDto
            {
                UserId = "mario777",
                Amount = 10,
                TransactionType = TransactionTypeEnum.Debit
            };

            var resp = await _client.PostAsJsonAsync("/api/transactions", dto);

            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var error = await resp.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            error.Should().NotBeNull();
            error!.ContainsKey("error").Should().BeTrue();
        }

        [Fact]
        public async Task Create_InvalidAmount_Returns400()
        {
            var dto = new CreateTransactionDto
            {
                UserId = "user-1",
                Amount = 0m,
                TransactionType = TransactionTypeEnum.Credit
            };

            var resp = await _client.PostAsJsonAsync("/api/transactions", dto);

            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_NegativeAmount_Returns400()
        {
            var dto = new CreateTransactionDto
            {
                UserId = "user-1",
                Amount = -100m,
                TransactionType = TransactionTypeEnum.Credit
            };

            var resp = await _client.PostAsJsonAsync("/api/transactions", dto);

            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAll_ReturnsPagedResult()
        {
            var resp = await _client.GetAsync("/api/transactions?pageNumber=1&pageSize=5");
            resp.EnsureSuccessStatusCode();

            var page = await resp.Content.ReadFromJsonAsync<PagedResult<TransactionDto>>();
            page.Should().NotBeNull();
            page!.Items.Should().NotBeEmpty();
            page.PageNumber.Should().Be(1);
            page.PageSize.Should().Be(5);
            page.TotalCount.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetById_Existing_Returns200()
        {
            var resp = await _client.GetAsync("/api/transactions/1");
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            var txn = await resp.Content.ReadFromJsonAsync<TransactionDto>();
            txn.Should().NotBeNull();
            txn!.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetById_NotFound_Returns404()
        {
            var resp = await _client.GetAsync("/api/transactions/100");
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}