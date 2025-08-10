using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using TransactionApp.Api.IntegrationTests.TestHost;
using TransactionApp.Application.DTOs;
using TransactionApp.Application.Utilities;

namespace TransactionApp.Api.IntegrationTests
{
    public class UsersControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        [Fact]
        public async Task Create_Successfull_Returns201_And_Body()
        {
            var dto = new CreateUserDto { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@test.com" };

            var resp = await _client.PostAsJsonAsync("/api/users", dto);

            resp.StatusCode.Should().Be(HttpStatusCode.Created);
            resp.Headers.Location.Should().NotBeNull();

            var body = await resp.Content.ReadFromJsonAsync<UserDto>();
            body.Should().NotBeNull();
            body!.Email.Should().Be(dto.Email);
            body.FirstName.Should().Be("Jane");
            body.LastName.Should().Be("Doe");
        }

        [Fact]
        public async Task Create_InvalidInputs_Returns400_WithErrors()
        {
            var dto = new CreateUserDto { FirstName = "", LastName = "", Email = "goodemail@gmail.com" };

            var resp = await _client.PostAsJsonAsync("/api/users", dto);

            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorList = await resp.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            errorList.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_InvalidEmail_Returns400_WithErrors()
        {
            var dto = new CreateUserDto { FirstName = "FirstName", LastName = "LastName", Email = "incorrectemailgmail.com" };

            var resp = await _client.PostAsJsonAsync("/api/users", dto);

            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorList = await resp.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            errorList.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAll_ReturnsPagedResult_With_Items()
        {
            var resp = await _client.GetAsync("/api/users?pageNumber=1&pageSize=5");

            resp.EnsureSuccessStatusCode();
            var paged = await resp.Content.ReadFromJsonAsync<PagedResult<UserDto>>();

            paged.Should().NotBeNull();
            paged!.Items.Should().NotBeEmpty();
            paged.PageNumber.Should().Be(1);
            paged.PageSize.Should().Be(5);
            paged.TotalCount.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetById_UserExisting_Returns200()
        {
            var resp = await _client.GetAsync("/api/users/user-1");

            resp.StatusCode.Should().Be(HttpStatusCode.OK);
            var user = await resp.Content.ReadFromJsonAsync<UserDto>();
            user.Should().NotBeNull();
            user!.Id.Should().Be("user-1");
        }

        [Fact]
        public async Task GetById_UserNotFound_Returns404()
        {
            var resp = await _client.GetAsync("/api/users/no-such-user");
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_Existing_Returns204_And_ChangesPersist()
        {
            var dto = new CreateUserDto { FirstName = "Renamed", LastName = "User", Email = "renamed@test.com" };

            var resp = await _client.PutAsJsonAsync("/api/users/user-2", dto);
            resp.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var get = await _client.GetAsync("/api/users/user-2");
            var user = await get.Content.ReadFromJsonAsync<UserDto>();
            user!.FirstName.Should().Be("Renamed");
            user.Email.Should().Be("renamed@test.com");
        }

        [Fact]
        public async Task Update_NotFound_Returns404()
        {
            var dto = new CreateUserDto { FirstName = "A", LastName = "B", Email = "a@test.com" };

            var resp = await _client.PutAsJsonAsync("/api/users/marioUser", dto);
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_InvalidDto_Returns400()
        {
            var dto = new CreateUserDto { FirstName = "FN", LastName = "LN", Email = "incorrectemailgmail" };

            var resp = await _client.PutAsJsonAsync("/api/users/user-3", dto);
            resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_Existing_Returns204()
        {
            var resp = await _client.DeleteAsync("/api/users/user-4");
            resp.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var check = await _client.GetAsync("/api/users/user-4");
            check.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotFound_Returns404()
        {
            var resp = await _client.DeleteAsync("/api/users/mario777");
            resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}