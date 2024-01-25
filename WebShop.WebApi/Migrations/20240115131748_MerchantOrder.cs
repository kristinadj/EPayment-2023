using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class MerchantOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                schema: "dbo",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Invoices_InvoiceId",
                schema: "dbo",
                table: "Orders");

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
                name: "InvoiceId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                schema: "dbo",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                schema: "dbo",
                table: "OrderItems",
                newName: "MerchantOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_OrderId",
                schema: "dbo",
                table: "OrderItems",
                newName: "IX_OrderItems_MerchantOrderId");

            migrationBuilder.CreateTable(
                name: "OrderMerchants",
                schema: "dbo",
                columns: table => new
                {
                    MerchantOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderMerchants", x => x.MerchantOrderId);
                    table.ForeignKey(
                        name: "FK_OrderMerchants_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "dbo",
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderMerchants_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "dbo",
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderMerchants_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "dbo",
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "106f44c6-caa8-48e1-a46d-5f0454176102", "AQAAAAEAACcQAAAAEB2zRQIwaOKBjmIC3Pf+x1bk/Uc1KrAd91LTugFDfu59o0r8RZHpqlVta1gysti+/Q==", "f59f187c-08d6-49f2-b5b8-295e3d93782a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f102c5c3-6d0b-49f1-a339-3a508c8c2000", "AQAAAAEAACcQAAAAEG/YfOnF/jbfI50whU62BoICZACCcT7JSgKXeyShoHxTMhgTT6eCwBzprP7v+zBttw==", "dd853222-b31f-4b04-8d1f-bb64564b50ae" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderMerchants_InvoiceId",
                schema: "dbo",
                table: "OrderMerchants",
                column: "InvoiceId",
                unique: true,
                filter: "[InvoiceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMerchants_MerchantId",
                schema: "dbo",
                table: "OrderMerchants",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMerchants_OrderId",
                schema: "dbo",
                table: "OrderMerchants",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_OrderMerchants_MerchantOrderId",
                schema: "dbo",
                table: "OrderItems",
                column: "MerchantOrderId",
                principalSchema: "dbo",
                principalTable: "OrderMerchants",
                principalColumn: "MerchantOrderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_OrderMerchants_MerchantOrderId",
                schema: "dbo",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "OrderMerchants",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "MerchantOrderId",
                schema: "dbo",
                table: "OrderItems",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_MerchantOrderId",
                schema: "dbo",
                table: "OrderItems",
                newName: "IX_OrderItems_OrderId");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                schema: "dbo",
                table: "Orders",
                type: "int",
                nullable: true);

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
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f7544ad6-8486-40aa-bed2-90e00d5add1b", "AQAAAAEAACcQAAAAEJhTGRpS3lPvgv6RsWlxxY/exmMnr5AGCIqL2FI61/yikHdJtTad7YBNsFE4x+d4qw==", "0b23d78c-b3d9-412a-8ccf-81b1986c99bf" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "61d66c5a-7ba1-413d-b430-9927d277277e", "AQAAAAEAACcQAAAAEFOsGTYtRwaXUcqC0AGmgxolFAHlU0MGeWhnsV8NniMwXCWvLGfK7IJ1CIOKGoQTbQ==", "fba7c3eb-6e45-41ee-a2a9-0531ea84ae31" });

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
                name: "FK_OrderItems_Orders_OrderId",
                schema: "dbo",
                table: "OrderItems",
                column: "OrderId",
                principalSchema: "dbo",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Invoices_InvoiceId",
                schema: "dbo",
                table: "Orders",
                column: "InvoiceId",
                principalSchema: "dbo",
                principalTable: "Invoices",
                principalColumn: "InvoiceId",
                onDelete: ReferentialAction.Restrict);

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
    }
}
