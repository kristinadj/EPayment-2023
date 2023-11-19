using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class TransactionredirectUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SenderAccountId",
                schema: "dbo",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "TransactionErrorUrl",
                schema: "dbo",
                table: "Transactions",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionFailureUrl",
                schema: "dbo",
                table: "Transactions",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionSuccessUrl",
                schema: "dbo",
                table: "Transactions",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionErrorUrl",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionFailureUrl",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionSuccessUrl",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "SenderAccountId",
                schema: "dbo",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
