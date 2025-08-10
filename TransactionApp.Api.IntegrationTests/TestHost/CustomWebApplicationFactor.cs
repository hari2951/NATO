using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TransactionApp.Infrastructure.Data;
using TransactionApp.Domain.Entities;
using TransactionApp.Domain.Enums;

namespace TransactionApp.Api.IntegrationTests.TestHost
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("UseInMemoryDb", "true");
            builder.UseSetting("InMemoryDbName", $"TestDb_{Guid.NewGuid()}");

            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
                Seed(ctx);
            });
        }

        private static void Seed(AppDbContext ctx)
        {
            var users = Enumerable.Range(1, 15).Select(i => new User
            {
                Id = $"user-{i}",
                FirstName = $"First{i}",
                LastName = $"Last{i}",
                Email = $"user{i}@test.com"
            }).ToList();

            ctx.Users.AddRange(users);

            var transactions = new List<Transaction>
            {
                new()
                {
                    UserId = "user-1",
                    Amount = 100.25m,
                    TransactionType = TransactionTypeEnum.Credit,
                    CreatedAt = new DateTime(DateTime.UtcNow.Year, 1, 10)
                },
                new()
                {
                    UserId = "user-1",
                    Amount = 50.00m,
                    TransactionType = TransactionTypeEnum.Debit,
                    CreatedAt = new DateTime(DateTime.UtcNow.Year, 2, 5)
                }
            };

            ctx.Transactions.AddRange(transactions);
            ctx.SaveChanges();
        }
    }
}