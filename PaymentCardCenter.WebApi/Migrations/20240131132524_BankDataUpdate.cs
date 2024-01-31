using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentCardCenter.WebApi.Migrations
{
    public partial class BankDataUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Banks",
                keyColumn: "BankId",
                keyValue: 2,
                column: "CardStartNumbers",
                value: "2024");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Banks",
                keyColumn: "BankId",
                keyValue: 2,
                column: "CardStartNumbers",
                value: "2023");
        }
    }
}
