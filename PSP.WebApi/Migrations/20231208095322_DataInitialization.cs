using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSP.WebApi.Migrations
{
    public partial class DataInitialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "dbo",
                table: "PaymentMethodMerchants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Secret",
                schema: "dbo",
                table: "PaymentMethodMerchants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "MerchantExternalId",
                schema: "dbo",
                table: "Merchants",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "dbo",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "dbo",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "IssuedToUserId",
                schema: "dbo",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
                table: "Merchants",
                columns: new[] { "MerchantId", "Address", "Email", "MerchantExternalId", "Name", "PhoneNumber", "ServiceName", "TransactionErrorUrl", "TransactionFailureUrl", "TransactionSuccessUrl" },
                values: new object[] { 1, "123 Main Street", "webshopadmin@lawpublishingagency.com", "408b89e8-e8e5-4b97-9c88-f19593d66378", "Law Publishing Web Shop", "+1 555-123-4567", "law-publishing-agency", "https://localhost:7295/invoice/@INVOICE_ID@/error", "https://localhost:7295/invoice/@INVOICE_ID@/failure", "https://localhost:7295/invoice/@INVOICE_ID@/success" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currencies",
                keyColumn: "CurrencyId",
                keyValue: 1);

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
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "dbo",
                table: "PaymentMethodMerchants");

            migrationBuilder.DropColumn(
                name: "Secret",
                schema: "dbo",
                table: "PaymentMethodMerchants");

            migrationBuilder.DropColumn(
                name: "Address",
                schema: "dbo",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "dbo",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "dbo",
                table: "Merchants");

            migrationBuilder.AlterColumn<int>(
                name: "MerchantExternalId",
                schema: "dbo",
                table: "Merchants",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "IssuedToUserId",
                schema: "dbo",
                table: "Invoices",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
