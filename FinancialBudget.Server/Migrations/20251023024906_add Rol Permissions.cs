using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinancialBudget.Server.Migrations
{
    /// <inheritdoc />
    public partial class addRolPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Icon",
                value: "bi bi-people");

            migrationBuilder.UpdateData(
                table: "RolOperations",
                keyColumn: "Id",
                keyValue: 22L,
                column: "OperationId",
                value: 2L);

            migrationBuilder.UpdateData(
                table: "RolOperations",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "OperationId", "RolId" },
                values: new object[] { 5L, 2L });

            migrationBuilder.UpdateData(
                table: "RolOperations",
                keyColumn: "Id",
                keyValue: 24L,
                column: "OperationId",
                value: 1L);

            migrationBuilder.InsertData(
                table: "RolOperations",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "OperationId", "RolId", "State", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 25L, new DateTimeOffset(new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -6, 0, 0, 0)), 1L, 2L, 3L, 1, null, null },
                    { 26L, new DateTimeOffset(new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -6, 0, 0, 0)), 1L, 5L, 3L, 1, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolOperations",
                keyColumn: "Id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "RolOperations",
                keyColumn: "Id",
                keyValue: 26L);

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Icon",
                value: "bi bi-list");

            migrationBuilder.UpdateData(
                table: "RolOperations",
                keyColumn: "Id",
                keyValue: 22L,
                column: "OperationId",
                value: 5L);

            migrationBuilder.UpdateData(
                table: "RolOperations",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "OperationId", "RolId" },
                values: new object[] { 2L, 3L });

            migrationBuilder.UpdateData(
                table: "RolOperations",
                keyColumn: "Id",
                keyValue: 24L,
                column: "OperationId",
                value: 5L);
        }
    }
}
