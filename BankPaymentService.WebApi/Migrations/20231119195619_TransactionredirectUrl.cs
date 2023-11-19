using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankPaymentService.WebApi.Migrations
{
    public partial class TransactionredirectUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionErrorUrl",
                schema: "dbo",
                table: "Invoices",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionFailureUrl",
                schema: "dbo",
                table: "Invoices",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionSuccessUrl",
                schema: "dbo",
                table: "Invoices",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionErrorUrl",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TransactionFailureUrl",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TransactionSuccessUrl",
                schema: "dbo",
                table: "Invoices");
        }
    }
}
