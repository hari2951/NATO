using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TransactionApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedingUserAndTransactionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName" },
                values: new object[,]
                {
                    { "10b42cd3-4690-4281-bc71-14679b0c7204", "alice@test.com", "Alice", "Smith" },
                    { "15cc7ba5-35c7-42dd-b688-d2bfa8c0f3a7", "bob@test.com", "Bob", "Johnson" },
                    { "3178e886-113f-44b8-9c63-2a173ddfe45c", "James@test.com", "James", "West" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CreatedAt", "TransactionType", "UserId" },
                values: new object[,]
                {
                    { 1, 100.00m, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "10b42cd3-4690-4281-bc71-14679b0c7204" },
                    { 2, 250.00m, new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "10b42cd3-4690-4281-bc71-14679b0c7204" },
                    { 3, 15.00m, new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "10b42cd3-4690-4281-bc71-14679b0c7204" },
                    { 4, 5.00m, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "10b42cd3-4690-4281-bc71-14679b0c7204" },
                    { 5, 50.00m, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "3178e886-113f-44b8-9c63-2a173ddfe45c" },
                    { 6, 75.00m, new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "3178e886-113f-44b8-9c63-2a173ddfe45c" },
                    { 7, 16.00m, new DateTime(2025, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "3178e886-113f-44b8-9c63-2a173ddfe45c" },
                    { 8, 22.00m, new DateTime(2025, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "3178e886-113f-44b8-9c63-2a173ddfe45c" },
                    { 9, 19.00m, new DateTime(2025, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "15cc7ba5-35c7-42dd-b688-d2bfa8c0f3a7" },
                    { 10, 79.00m, new DateTime(2025, 5, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "15cc7ba5-35c7-42dd-b688-d2bfa8c0f3a7" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "10b42cd3-4690-4281-bc71-14679b0c7204");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "15cc7ba5-35c7-42dd-b688-d2bfa8c0f3a7");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3178e886-113f-44b8-9c63-2a173ddfe45c");
        }
    }
}
