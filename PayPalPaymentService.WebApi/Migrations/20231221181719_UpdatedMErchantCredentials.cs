using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayPalPaymentService.WebApi.Migrations
{
    public partial class UpdatedMErchantCredentials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                column: "Secret",
                value: "EAnL13M1IeQPT2YTsdwgmrO1R9RI97mqWFnF7mD7WQULXL6fmiTWIn9pDI1X6aAdK6leDX0RLdjM7tDh");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                column: "Secret",
                value: "AV3Z4kgHI5D0Dbmcx9Aad6ES4BNG2TgPSipgEEtc2x0sq44FjQcDD3nbbKd9swsAz2wuW_HLKu6L64uh");
        }
    }
}
