using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank2.WebApi.Migrations
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
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1,
                column: "AccountNumber",
                value: "123-0000009876123-40");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 2,
                column: "AccountNumber",
                value: "123-0000000040987-65");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "PanNumber",
                value: "abaaaf0d6042a0916087da6065f84e4c1614b23d513daec6f5effc8987887832");
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
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1,
                column: "AccountNumber",
                value: "987612340");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 2,
                column: "AccountNumber",
                value: "1234098765");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "PanNumber",
                value: "2023 5678 9012 3456");
        }
    }
}
