using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayPalPaymentService.WebApi.Migrations
{
    public partial class Merchant2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                column: "Secret",
                value: "W0cagwccKGE0DM8pfTJbN3BwypRB1oCFOitaKMae5e5Y4m3jm706vL5UcmsfWKBg+unP8DjjYFt+JLwiVYCwpB//BCyezLBL+l5i3Z5FgOKqAey9gi1jZw+XSpjUMt+M");

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Merchants",
                columns: new[] { "MerchantId", "ClientId", "PaymentServiceMerchantId", "Secret" },
                values: new object[] { 2, "", 3, "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 2);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                column: "Secret",
                value: "W0cagwccKGE0DM8pfTJbN3BwypRB1oCFOitaKMae5e5Y4m3jm706vL5UcmsfWKBg+unP8DjjYFt+JLwiVYCwpB//BCyezLBL+l5i3Z5FgOKqAey9gi1jZw+XSpjUMt+M");
        }
    }
}
