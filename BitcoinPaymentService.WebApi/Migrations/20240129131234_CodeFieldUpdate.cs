using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitcoinPaymentService.WebApi.Migrations
{
    public partial class CodeFieldUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CryptoCloudShopId",
                schema: "dbo",
                table: "Merchants",
                newName: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                schema: "dbo",
                table: "Merchants",
                newName: "CryptoCloudShopId");
        }
    }
}
