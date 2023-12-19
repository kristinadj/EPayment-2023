using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class FieldUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Invoices_InvoiceId",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_InvoiceId",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "648899a7-fbd1-4a94-b127-2f981f66179d", "AQAAAAEAACcQAAAAEFyH/fsXqFhK0BRrPv+N4lpPdHhxuDtSLM3wFsKtPOExNTxKYrSmDkH8fWY+OOLoog==", "ce1263ae-278c-4f2d-bc08-5b2217832b15" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3b99c95f-861c-4109-8f85-e1bf7d6ea8d3", "AQAAAAEAACcQAAAAEKtUAcAcBr3xbhveaxSVkm8S0ZvaK2kcHLR72SzauOm9TRj8TXT3WISmW+dut/66cA==", "60800aa1-729d-47f1-82c7-986db280bac4" });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TransactionId",
                schema: "dbo",
                table: "Invoices",
                column: "TransactionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Transactions_TransactionId",
                schema: "dbo",
                table: "Invoices",
                column: "TransactionId",
                principalSchema: "dbo",
                principalTable: "Transactions",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Transactions_TransactionId",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_TransactionId",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                schema: "dbo",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                schema: "dbo",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "478f0bd8-4a85-49c3-84a1-ef22339c9942", "AQAAAAEAACcQAAAAEAmkPlnsEdMrZ/qReCerRlQ9nxMGnvVFhl0fGzc0GxtZFmYKvy9CU5swrYf9khX6/w==", "12beb959-5229-4988-98f0-0dc38779cc48" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "64ad320f-970b-4bc5-8c46-ab91c7723c74", "AQAAAAEAACcQAAAAEMYxA7pG4G3crgMOlIiqtyovFIqsfqLsqbSOU+qbJffYEo1fl4tkKuN/1IbsFRrOhw==", "510fd157-3d91-4b13-b258-b54f7b64d173" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_InvoiceId",
                schema: "dbo",
                table: "Transactions",
                column: "InvoiceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Invoices_InvoiceId",
                schema: "dbo",
                table: "Transactions",
                column: "InvoiceId",
                principalSchema: "dbo",
                principalTable: "Invoices",
                principalColumn: "InvoiceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
