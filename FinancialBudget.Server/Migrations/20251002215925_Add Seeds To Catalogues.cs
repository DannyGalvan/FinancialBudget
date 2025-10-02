using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinancialBudget.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedsToCatalogues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Origins",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Name", "State", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1L, new DateTimeOffset(new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1L, "Mantenimiento", 1, null, null },
                    { 2L, new DateTimeOffset(new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1L, "Eventos", 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "Priorities",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Name", "State", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1L, new DateTimeOffset(new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1L, "Baja", 1, null, null },
                    { 2L, new DateTimeOffset(new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1L, "Media", 1, null, null },
                    { 3L, new DateTimeOffset(new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1L, "Alta", 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "RequestStatuses",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Name", "State", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1L, new DateTimeOffset(new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1L, "Pendiente", 1, null, null },
                    { 2L, new DateTimeOffset(new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1L, "Aprobado", 1, null, null },
                    { 3L, new DateTimeOffset(new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1L, "Rechazado", 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "SplitTypes",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Name", "State", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1L, new DateTimeOffset(new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1L, "Debito", 1, null, null },
                    { 2L, new DateTimeOffset(new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1L, "Credito", 1, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Origins",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Origins",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Priorities",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Priorities",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Priorities",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "RequestStatuses",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "RequestStatuses",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "RequestStatuses",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "SplitTypes",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "SplitTypes",
                keyColumn: "Id",
                keyValue: 2L);
        }
    }
}
