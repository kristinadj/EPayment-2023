using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class UpdatedCurrencyData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 2,
                column: "CurrencyId",
                value: 3);

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Accounts",
                columns: new[] { "AccountId", "AccountNumber", "Balance", "CurrencyId", "OwnerId" },
                values: new object[] { 3, "105-0000000000001-37", 500.0, 2, 1 });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "PanNumber",
                value: "lCeUSOrMw0PwocsTtLCWKvE0C+tQi0H/fHA/8R4tGBE=");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "BusinessCustomers",
                keyColumn: "BusinessCustomerId",
                keyValue: 1,
                columns: new[] { "DefaultAccountId", "Password" },
                values: new object[] { 3, "0hqo5asTUiTJe/2ZcF3vuA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 2,
                column: "CurrencyId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "BusinessCustomers",
                keyColumn: "BusinessCustomerId",
                keyValue: 1,
                columns: new[] { "DefaultAccountId", "Password" },
                values: new object[] { 1, "0hqo5asTUiTJe/2ZcF3vuA==" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "PanNumber",
                value: "lCeUSOrMw0PwocsTtLCWKvE0C+tQi0H/fHA/8R4tGBE=");
        }
    }
}
