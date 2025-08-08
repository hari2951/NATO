using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TransactionApp.Domain.Entities;
using TransactionApp.Domain.Enums;

namespace TransactionApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            var users = new List<User>
            {
                new() {
                    Id = "10b42cd3-4690-4281-bc71-14679b0c7204",
                    FirstName = "Alice",
                    LastName = "Smith",
                    Email = "alice@test.com"
                },
                new() {
                    Id = "15cc7ba5-35c7-42dd-b688-d2bfa8c0f3a7",
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Email = "bob@test.com"
                },
                new() {
                     Id = "3178e886-113f-44b8-9c63-2a173ddfe45c",
                     FirstName = "James",
                     LastName = "West",
                     Email = "James@test.com"
                 },
            };

            modelBuilder.Entity<User>().HasData(users);

            var transactions = new List<Transaction>
            {
                new() {
                    Id = 1,
                    UserId = "10b42cd3-4690-4281-bc71-14679b0c7204",
                    Amount = 100.00m,
                    TransactionType = TransactionTypeEnum.Debit,
                    CreatedAt = new DateTime(2025, 1, 10)
                },
                new() {
                    Id = 2,
                    UserId = "10b42cd3-4690-4281-bc71-14679b0c7204",
                    Amount = 250.00m,
                    TransactionType = TransactionTypeEnum.Credit,
                    CreatedAt = new DateTime(2025, 2, 15)
                },
                new() {
                    Id = 3,
                    UserId = "10b42cd3-4690-4281-bc71-14679b0c7204",
                    Amount = 15.00m,
                    TransactionType = TransactionTypeEnum.Debit,
                    CreatedAt = new DateTime(2025, 7, 15)
                },
                new() {
                    Id = 4,
                    UserId = "10b42cd3-4690-4281-bc71-14679b0c7204",
                    Amount = 5.00m,
                    TransactionType = TransactionTypeEnum.Debit,
                    CreatedAt = new DateTime(2025, 6, 01)
                },
                new() {
                    Id = 5,
                    UserId = "3178e886-113f-44b8-9c63-2a173ddfe45c",
                    Amount = 50.00m,
                    TransactionType = TransactionTypeEnum.Credit,
                    CreatedAt = new DateTime(2025, 8, 01)
                },
                new() {
                    Id = 6,
                    UserId = "3178e886-113f-44b8-9c63-2a173ddfe45c",
                    Amount = 75.00m,
                    TransactionType = TransactionTypeEnum.Credit,
                    CreatedAt = new DateTime(2025, 4, 20)
                },
                new() {
                    Id = 7,
                    UserId = "3178e886-113f-44b8-9c63-2a173ddfe45c",
                    Amount = 16.00m,
                    TransactionType = TransactionTypeEnum.Credit,
                    CreatedAt = new DateTime(2025, 3, 13)
                },
                new() {
                    Id = 8,
                    UserId = "3178e886-113f-44b8-9c63-2a173ddfe45c",
                    Amount = 22.00m,
                    TransactionType = TransactionTypeEnum.Debit,
                    CreatedAt = new DateTime(2025, 6, 04)
                },
                new() {
                    Id = 9,
                    UserId = "15cc7ba5-35c7-42dd-b688-d2bfa8c0f3a7",
                    Amount = 19.00m,
                    TransactionType = TransactionTypeEnum.Credit,
                    CreatedAt = new DateTime(2025, 5, 13)
                },
                new() {
                    Id = 10,
                    UserId = "15cc7ba5-35c7-42dd-b688-d2bfa8c0f3a7",
                    Amount = 79.00m,
                    TransactionType = TransactionTypeEnum.Debit,
                    CreatedAt = new DateTime(2025, 5, 29)
                }
            };

            modelBuilder.Entity<Transaction>().HasData(transactions);

            base.OnModelCreating(modelBuilder);
        }

        private static DateTime RandomDateInCurrentYear(Random random)
        {
            var start = new DateTime(DateTime.Now.Year, 1, 1);
            var range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }
    }
}
