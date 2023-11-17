using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class PspPaymentMethodId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_InvoiceId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "PspPaymentMethodId",
                schema: "dbo",
                table: "PaymentMethods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                schema: "dbo",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MerchantId",
                schema: "dbo",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "737b9101-4845-4058-9d2d-6f5bcd646edb", "AQAAAAEAACcQAAAAEL9UIqlmb2lHgebPLrDz5E2XHZBlIQSwteh51UvwsKdBhVkm3Rm0/ikJSYj6XSdavg==", "427b3795-7fc2-434c-b6c0-007a11ee21fd" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_InvoiceId",
                schema: "dbo",
                table: "Orders",
                column: "InvoiceId",
                unique: true,
                filter: "[InvoiceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MerchantId",
                schema: "dbo",
                table: "Orders",
                column: "MerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Merchants_MerchantId",
                schema: "dbo",
                table: "Orders",
                column: "MerchantId",
                principalSchema: "dbo",
                principalTable: "Merchants",
                principalColumn: "MerchantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Merchants_MerchantId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_InvoiceId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_MerchantId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PspPaymentMethodId",
                schema: "dbo",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                schema: "dbo",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5193d1cf-43a3-4d37-ac8c-14237726748b", "AQAAAAEAACcQAAAAEHiL1hiAsqmiRA6wjPSR9qdFJZFOUtzakcKHM6EqOaYvHYx1U2m9/SnIfJvRXw8Nxw==", "e95c438c-984a-489c-85ed-1a87aa86890b" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_InvoiceId",
                schema: "dbo",
                table: "Orders",
                column: "InvoiceId",
                unique: true);
        }
    }
}
