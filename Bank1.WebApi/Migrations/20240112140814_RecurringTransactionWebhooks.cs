using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class RecurringTransactionWebhooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecurringTransactionFailureUrl",
                schema: "dbo",
                table: "RecurringTransactionDefinitions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurringTransactionSuccessUrl",
                schema: "dbo",
                table: "RecurringTransactionDefinitions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "BusinessCustomers",
                keyColumn: "BusinessCustomerId",
                keyValue: 1,
                column: "Password",
                value: "0hqo5asTUiTJe/2ZcF3vuA==");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "PanNumber",
                value: "lCeUSOrMw0PwocsTtLCWKvE0C+tQi0H/fHA/8R4tGBE=");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecurringTransactionFailureUrl",
                schema: "dbo",
                table: "RecurringTransactionDefinitions");

            migrationBuilder.DropColumn(
                name: "RecurringTransactionSuccessUrl",
                schema: "dbo",
                table: "RecurringTransactionDefinitions");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "BusinessCustomers",
                keyColumn: "BusinessCustomerId",
                keyValue: 1,
                column: "Password",
                value: "0hqo5asTUiTJe/2ZcF3vuA==");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "PanNumber",
                value: "lCeUSOrMw0PwocsTtLCWKvE0C+tQi0H/fHA/8R4tGBE=");
        }
    }
}
