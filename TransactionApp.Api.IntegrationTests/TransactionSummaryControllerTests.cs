using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using TransactionApp.Api.IntegrationTests.TestHost;
using TransactionApp.Application.DTOs;
using TransactionApp.Application.Utilities;
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
        public async Task GetAllTransactions_PerUser_ReturnsPagedResults()
        {
            // Act
            var resp = await _client.GetAsync("/api/transactionsummary/total-per-user?pageNumber=1&pageSize=5");

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await resp.Content.ReadFromJsonAsync<PagedResult<UserTotalDto>>();

            result.Should().NotBeNull();
            result!.Items.Should().NotBeEmpty();
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(5);
            result.TotalCount.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetAllTransactions_PerType_ReturnsPagedResults()
        {
            // Act
            var resp = await _client.GetAsync("/api/transactionsummary/total-per-type?pageNumber=1&pageSize=5");

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await resp.Content.ReadFromJsonAsync<PagedResult<TransactionTypeTotalDto>>();

            result.Should().NotBeNull();
            result!.Items.Should().NotBeEmpty();
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(5);
            result.TotalCount.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetHighVolumeTransactions_WithValidThreshold_ReturnsPagedResults()
        {
            // Act
            var resp = await _client.GetAsync("/api/transactionsummary/high-volume?threshold=50&pageNumber=1&pageSize=5");

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await resp.Content.ReadFromJsonAsync<PagedResult<TransactionDto>>();

            result.Should().NotBeNull();
            result!.Items.Should().OnlyContain(t => t.Amount > 50);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(5);
        }

        [Fact]
        public async Task GetHighVolumeTransactions_WithInvalidThreshold_ReturnsBadRequest()
        {
            // Act
            var resp = await _client.GetAsync("/api/transactionsummary/high-volume?threshold=0");

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = await resp.Content.ReadAsStringAsync();
            error.Should().Contain("Threshold");
        }
    }
}
