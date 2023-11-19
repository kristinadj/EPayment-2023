using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class DataInitialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expiratorydate",
                schema: "dbo",
                table: "Cards",
                newName: "ExpiratoryDate");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PanNumber",
                schema: "dbo",
                table: "Cards",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<int>(
                name: "CVV",
                schema: "dbo",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                schema: "dbo",
                table: "Accounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Currencies",
                columns: new[] { "CurrencyId", "Code", "Name", "Symbol" },
                values: new object[,]
                {
                    { 1, "RSD", "Serbian Dinar", "RSD" },
                    { 2, "EUR", "Euro", "€" },
                    { 3, "USD", "American Dollar", "$" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "123 Main Street", "webshopadmin@lawpublishingagency.com", "Law Publishing Web Shop", "", "+1 555-123-4567" },
                    { 2, "789 Elm Street,", "johndoe@email.com", "John", "Doe", "+1 555-987-6543" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Accounts",
                columns: new[] { "AccountId", "AccountNumber", "Balance", "CurrencyId", "OwnerId" },
                values: new object[,]
                {
                    { 1, "9876543210", 14500.0, 1, 1 },
                    { 2, "1234567890", 6530.0, 1, 2 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "BusinessCustomers",
                columns: new[] { "BusinessCustomerId", "CustomerId", "Password" },
                values: new object[] { 1, 1, "LPAPassword5!" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "ExchangeRates",
                columns: new[] { "ExchangeRateId", "FromCurrencyId", "Rate", "ToCurrencyId" },
                values: new object[,]
                {
                    { 1, 1, 0.0085000000000000006, 2 },
                    { 2, 1, 0.0092999999999999992, 3 },
                    { 3, 2, 116.94, 1 },
                    { 4, 2, 1.0900000000000001, 3 },
                    { 5, 3, 107.53, 1 },
                    { 6, 3, 0.92000000000000004, 2 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Cards",
                columns: new[] { "CardId", "AccountId", "CVV", "CardHolderName", "ExpiratoryDate", "PanNumber" },
                values: new object[] { 1, 2, 123, "JOHN DOE", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1234567890123456" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "BusinessCustomers",
                keyColumn: "BusinessCustomerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ExchangeRates",
                keyColumn: "ExchangeRateId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ExchangeRates",
                keyColumn: "ExchangeRateId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ExchangeRates",
                keyColumn: "ExchangeRateId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ExchangeRates",
                keyColumn: "ExchangeRateId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ExchangeRates",
                keyColumn: "ExchangeRateId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ExchangeRates",
                keyColumn: "ExchangeRateId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currencies",
                keyColumn: "CurrencyId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currencies",
                keyColumn: "CurrencyId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currencies",
                keyColumn: "CurrencyId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "dbo",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CVV",
                schema: "dbo",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "ExpiratoryDate",
                schema: "dbo",
                table: "Cards",
                newName: "Expiratorydate");

            migrationBuilder.AlterColumn<string>(
                name: "PanNumber",
                schema: "dbo",
                table: "Cards",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                schema: "dbo",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
