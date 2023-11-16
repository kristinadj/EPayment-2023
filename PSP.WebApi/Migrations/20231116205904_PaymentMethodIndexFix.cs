using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSP.WebApi.Migrations
{
    public partial class PaymentMethodIndexFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_ServiceName",
                schema: "dbo",
                table: "PaymentMethods");

            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                schema: "dbo",
                table: "Merchants",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_ServiceName_ServiceApiSufix",
                schema: "dbo",
                table: "PaymentMethods",
                columns: new[] { "ServiceName", "ServiceApiSufix" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_ServiceName_ServiceApiSufix",
                schema: "dbo",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "ServiceName",
                schema: "dbo",
                table: "Merchants");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_ServiceName",
                schema: "dbo",
                table: "PaymentMethods",
                column: "ServiceName",
                unique: true);
        }
    }
}
