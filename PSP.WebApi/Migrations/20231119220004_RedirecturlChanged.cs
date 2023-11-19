using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSP.WebApi.Migrations
{
    public partial class RedirecturlChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                columns: new[] { "TransactionErrorUrl", "TransactionFailureUrl", "TransactionSuccessUrl" },
                values: new object[] { "https://localhost:7295/invoice/@INVOICE_ID@/error", "https://localhost:7295/invoice/@INVOICE_ID@/failure", "https://localhost:7295/invoice/@INVOICE_ID@/success" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                columns: new[] { "TransactionErrorUrl", "TransactionFailureUrl", "TransactionSuccessUrl" },
                values: new object[] { "/invoice/@INVOICE_ID@/error", "/invoice/@INVOICE_ID@/failure", "/invoice/@INVOICE_ID@/success" });
        }
    }
}
