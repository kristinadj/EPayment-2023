using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayPalPaymentService.WebApi.Migrations
{
    public partial class EncryptColumn : Migration
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                column: "Secret",
                value: "EAnL13M1IeQPT2YTsdwgmrO1R9RI97mqWFnF7mD7WQULXL6fmiTWIn9pDI1X6aAdK6leDX0RLdjM7tDh");
        }
    }
}
