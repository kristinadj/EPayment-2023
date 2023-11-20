using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class PanNumberUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PanNumber",
                schema: "dbo",
                table: "Cards",
                type: "nvarchar(19)",
                maxLength: 19,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "PanNumber",
                value: "1234 5678 9012 3456");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PanNumber",
                schema: "dbo",
                table: "Cards",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(19)",
                oldMaxLength: 19);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "PanNumber",
                value: "1234567890123456");
        }
    }
}
