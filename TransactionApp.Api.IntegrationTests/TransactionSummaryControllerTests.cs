using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using TransactionApp.Api.IntegrationTests.TestHost;
using TransactionApp.Application.DTOs;
using TransactionApp.Domain.Enums;

namespace TransactionApp.Api.IntegrationTests
{
    public class TransactionSummaryControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TransactionSummaryControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetByUserAndType_WithType_ReturnsTotalForType()
        {
            var url = "/api/TransactionSummary/by-user-and-type?userId=user-1&transactionType=Credit";

            var resp = await _client.GetAsync(url);

            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            var summary = await resp.Content.ReadFromJsonAsync<TransactionSummaryDto>();
            summary.Should().NotBeNull();
            summary!.UserId.Should().Be("user-1");
            summary.TransactionType.Should().Be(TransactionTypeEnum.Credit);
            summary.TotalAmount.Should().Be(100.25m);
        }

        [Fact]
        public async Task GetByUserAndType_NoType_ReturnsTotalForAll()
        {
            var url = "/api/TransactionSummary/by-user-and-type?userId=user-1";

            var resp = await _client.GetAsync(url);
            resp.EnsureSuccessStatusCode();

            var summary = await resp.Content.ReadFromJsonAsync<TransactionSummaryDto>();
            summary.Should().NotBeNull();
            summary!.UserId.Should().Be("user-1");
            summary.TransactionType.Should().BeNull();
            summary.TotalAmount.Should().Be(150.25m);
        }

       
        [Fact]
        public async Task GetByUserAndType_WithDateRange_FiltersCorrectly()
        {
            var start = new DateTime(DateTime.UtcNow.Year, 2, 1).ToString("O");
            var end = new DateTime(DateTime.UtcNow.Year, 2, 28).ToString("O");

            var url = $"/api/TransactionSummary/by-user-and-type?userId=user-1&startDate={Uri.EscapeDataString(start)}&endDate={Uri.EscapeDataString(end)}";

            var resp = await _client.GetAsync(url);
            resp.EnsureSuccessStatusCode();

            var summary = await resp.Content.ReadFromJsonAsync<TransactionSummaryDto>();
            summary.Should().NotBeNull();
            summary!.TotalAmount.Should().Be(50.00m);
        }

        [Fact]
        public async Task GetByUserAndType_MissingUserId_Returns400()
        {
            var url = "/api/TransactionSummary/by-user-and-type";
            var resp = await _client.GetAsync(url);

            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await resp.Content.ReadAsStringAsync();
            body.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetByUserAndType_NonExistentUser_ReturnsZeroTotal()
        {
            var url = "/api/TransactionSummary/by-user-and-type?userId=mario";
            var resp = await _client.GetAsync(url);
            resp.EnsureSuccessStatusCode();

            var summary = await resp.Content.ReadFromJsonAsync<TransactionSummaryDto>();
            summary.Should().NotBeNull();
            summary!.UserId.Should().Be("mario");
            summary.TotalAmount.Should().Be(0m);
        }
    }
}
