using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitcoinPaymentService.WebApi.Migrations
{
    public partial class CrypotCloudUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "PairingCode",
                schema: "dbo",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Token",
                schema: "dbo",
                table: "Merchants");

            migrationBuilder.RenameColumn(
                name: "BitPayId",
                schema: "dbo",
                table: "Invoices",
                newName: "ExternalPaymentServiceInvoiceId");

            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                schema: "dbo",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CryptoCloudShopId",
                schema: "dbo",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKey",
                schema: "dbo",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "CryptoCloudShopId",
                schema: "dbo",
                table: "Merchants");

            migrationBuilder.RenameColumn(
                name: "ExternalPaymentServiceInvoiceId",
                schema: "dbo",
                table: "Invoices",
                newName: "BitPayId");

            migrationBuilder.AddColumn<string>(
                name: "PairingCode",
                schema: "dbo",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                schema: "dbo",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Merchants",
                columns: new[] { "MerchantId", "PairingCode", "PaymentServiceMerchantId", "Token" },
                values: new object[] { 1, "", 2, "" });
        }
    }
}
