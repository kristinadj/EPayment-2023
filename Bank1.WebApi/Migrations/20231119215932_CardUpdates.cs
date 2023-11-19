using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class CardUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExpiratoryDate",
                schema: "dbo",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "ExpiratoryDate",
                value: "12/25");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiratoryDate",
                schema: "dbo",
                table: "Cards",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "ExpiratoryDate",
                value: new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
