using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankPaymentService.WebApi.Migrations
{
    public partial class BankUrlFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Banks",
                keyColumn: "BankId",
                keyValue: 1,
                column: "RedirectUrl",
                value: "https://localhost:7092/api");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Banks",
                keyColumn: "BankId",
                keyValue: 1,
                column: "RedirectUrl",
                value: "https://localhost:7092/");
        }
    }
}
