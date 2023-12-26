using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class HashedPan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PanNumber",
                schema: "dbo",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(19)",
                oldMaxLength: 19);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "BusinessCustomers",
                keyColumn: "BusinessCustomerId",
                keyValue: 1,
                column: "Password",
                value: "0hqo5asTUiTJe/2ZcF3vuA==");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "PanNumber",
                value: "db7d00d5266ec0ba6808f27a40f8a376ace7e718847d9eba813a2a40adf8ab1c");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PanNumber",
                schema: "dbo",
                table: "Cards",
                type: "nvarchar(19)",
                maxLength: 19,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "BusinessCustomers",
                keyColumn: "BusinessCustomerId",
                keyValue: 1,
                column: "Password",
                value: "0hqo5asTUiTJe/2ZcF3vuA==");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "PanNumber",
                value: "1234 5678 9012 3456");
        }
    }
}
