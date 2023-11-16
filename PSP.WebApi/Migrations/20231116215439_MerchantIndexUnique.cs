using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSP.WebApi.Migrations
{
    public partial class MerchantIndexUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Merchants_ServiceName_MerchantExternalId",
                schema: "dbo",
                table: "Merchants",
                columns: new[] { "ServiceName", "MerchantExternalId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Merchants_ServiceName_MerchantExternalId",
                schema: "dbo",
                table: "Merchants");
        }
    }
}
