using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankPaymentService.WebApi.Migrations
{
    public partial class PreferredAccountNumberUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                column: "PreferredAccountNumber",
                value: "106-0000000000000-30");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                column: "PreferredAccountNumber",
                value: "9876543210");
        }
    }
}
