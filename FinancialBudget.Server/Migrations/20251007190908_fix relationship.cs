using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialBudget.Server.Migrations
{
    /// <inheritdoc />
    public partial class fixrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Traceabilities_Users_UserId",
                table: "Traceabilities");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Traceabilities",
                newName: "CreateUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Traceabilities_UserId",
                table: "Traceabilities",
                newName: "IX_Traceabilities_CreateUserId");

            migrationBuilder.AddColumn<long>(
                name: "AuthorizeUserId",
                table: "Traceabilities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Traceabilities_AuthorizeUserId",
                table: "Traceabilities",
                column: "AuthorizeUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Traceabilities_Users_AuthorizeUserId",
                table: "Traceabilities",
                column: "AuthorizeUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Traceabilities_Users_CreateUserId",
                table: "Traceabilities",
                column: "CreateUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Traceabilities_Users_AuthorizeUserId",
                table: "Traceabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Traceabilities_Users_CreateUserId",
                table: "Traceabilities");

            migrationBuilder.DropIndex(
                name: "IX_Traceabilities_AuthorizeUserId",
                table: "Traceabilities");

            migrationBuilder.DropColumn(
                name: "AuthorizeUserId",
                table: "Traceabilities");

            migrationBuilder.RenameColumn(
                name: "CreateUserId",
                table: "Traceabilities",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Traceabilities_CreateUserId",
                table: "Traceabilities",
                newName: "IX_Traceabilities_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Traceabilities_Users_UserId",
                table: "Traceabilities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
