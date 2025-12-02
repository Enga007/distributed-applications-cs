using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SafeBG.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "SeverityLevel" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 26, 23, 22, 12, 23, DateTimeKind.Utc).AddTicks(7922), null, true, "Опасно куче", 0 },
                    { 2, new DateTime(2025, 11, 26, 23, 22, 12, 23, DateTimeKind.Utc).AddTicks(8504), null, true, "Паднало дърво", 0 },
                    { 3, new DateTime(2025, 11, 26, 23, 22, 12, 23, DateTimeKind.Utc).AddTicks(8507), null, true, "Разлят боклук", 0 },
                    { 4, new DateTime(2025, 11, 26, 23, 22, 12, 23, DateTimeKind.Utc).AddTicks(8508), null, true, "ПТП", 0 },
                    { 5, new DateTime(2025, 11, 26, 23, 22, 12, 23, DateTimeKind.Utc).AddTicks(8509), null, true, "Опасна сграда", 0 },
                    { 6, new DateTime(2025, 11, 26, 23, 22, 12, 23, DateTimeKind.Utc).AddTicks(8510), null, true, "Изчезнал човек", 0 },
                    { 7, new DateTime(2025, 11, 26, 23, 22, 12, 23, DateTimeKind.Utc).AddTicks(8511), null, true, "Престъпна дейност", 0 },
                    { 8, new DateTime(2025, 11, 26, 23, 22, 12, 23, DateTimeKind.Utc).AddTicks(8513), null, true, "Пожар", 0 },
                    { 9, new DateTime(2025, 11, 26, 23, 22, 12, 23, DateTimeKind.Utc).AddTicks(8514), null, true, "Природно бедствие", 0 },
                    { 10, new DateTime(2025, 11, 26, 23, 22, 12, 23, DateTimeKind.Utc).AddTicks(8515), null, true, "Друг проблем", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
