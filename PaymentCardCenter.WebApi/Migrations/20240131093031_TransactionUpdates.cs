using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentCardCenter.WebApi.Migrations
{
    public partial class TransactionUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AquirerTransctionId",
                schema: "dbo",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AquirerBankId",
                schema: "dbo",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumberStartNumbers",
                schema: "dbo",
                table: "Banks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Banks",
                keyColumn: "BankId",
                keyValue: 1,
                column: "AccountNumberStartNumbers",
                value: "105");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Banks",
                keyColumn: "BankId",
                keyValue: 2,
                column: "AccountNumberStartNumbers",
                value: "123");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumberStartNumbers",
                schema: "dbo",
                table: "Banks");

            migrationBuilder.AlterColumn<int>(
                name: "AquirerTransctionId",
                schema: "dbo",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AquirerBankId",
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
