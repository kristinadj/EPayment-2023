using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayPalPaymentService.WebApi.Migrations
{
    public partial class PaymentServiceMerchantId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                column: "PaymentServiceMerchantId",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                column: "PaymentServiceMerchantId",
                value: 1);
        }
    }
}
