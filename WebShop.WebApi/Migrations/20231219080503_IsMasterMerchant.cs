using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class IsMasterMerchant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentMethodMerchants",
                schema: "dbo");

            migrationBuilder.AddColumn<bool>(
                name: "IsMasterMerchant",
                schema: "dbo",
                table: "Merchants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "83332538-26e3-4d0a-be5d-c8b57f185d0f", "AQAAAAEAACcQAAAAEIlGrjTH3B6pnIePoiHku23nSRkunVjNdqVVzaR1OFZljBIS8fLN4aF2xkSiXv3L/w==", "c6ede1ba-1622-4fe8-9cca-554f64775866" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2e87d106-2e43-4a19-bd4c-843920dcf3e9", 0, "456 Oak Avenue", "a90e7c1c-4581-48af-b096-245282ad1a0f", "agencyadmin@legaldocsagency.com", false, false, null, "Legal Documents Agency", "AGENCYADMIN@LEGALDOCSAGENCY.COM", null, "AQAAAAEAACcQAAAAEFyD5Ie7BOJHmtZuOTT2NHo0MuRxzAdLBvMsFEhw6E7glFd8vpmqjw7EjwDIU2qQKQ==", "+1 555-987-6543", false, 0, "8531345f-4415-4b56-9eb6-9c190c9d709b", false, null });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                column: "IsMasterMerchant",
                value: true);

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Merchants",
                columns: new[] { "MerchantId", "IsMasterMerchant", "PspMerchantId", "UserId" },
                values: new object[] { 2, false, null, "2e87d106-2e43-4a19-bd4c-843920dcf3e9" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Items",
                columns: new[] { "ItemId", "CurrencyId", "Description", "MerchantId", "Name", "Price" },
                values: new object[,]
                {
                    { 11, 2, "Professional drafting of legal contracts.", 2, "Contract Drafting Services", 600.0 },
                    { 12, 2, "Assistance with trademark registration.", 2, "Trademark Registration", 900.0 },
                    { 13, 2, "Support for filing patents.", 2, "Patent Filing Support", 750.0 },
                    { 14, 2, "Accurate translations of legal documents.", 2, "Legal Translations", 150.0 },
                    { 15, 2, "Consultation on corporate governance.", 2, "Corporate Governance Advisory", 700.0 },
                    { 16, 2, "Thorough legal research assistance.", 2, "Legal Research Services", 550.0 },
                    { 17, 2, "Ensuring compliance with data privacy laws.", 2, "Data Privacy Compliance", 800.0 },
                    { 18, 2, "Expertise in international legal matters.", 2, "International Law Consultation", 850.0 },
                    { 19, 2, "Assistance in resolving legal disputes.", 2, "Dispute Resolution Services", 700.0 },
                    { 20, 2, "Seminars on various legal topics.", 2, "Legal Training Seminars", 750.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9");

            migrationBuilder.DropColumn(
                name: "IsMasterMerchant",
                schema: "dbo",
                table: "Merchants");

            migrationBuilder.CreateTable(
                name: "PaymentMethodMerchants",
                schema: "dbo",
                columns: table => new
                {
                    PaymentMethodMerchantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethodMerchants", x => x.PaymentMethodMerchantId);
                    table.ForeignKey(
                        name: "FK_PaymentMethodMerchants_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "dbo",
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentMethodMerchants_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "dbo",
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6c4de0d5-96fe-4a82-940f-ea5071b1d544", "AQAAAAEAACcQAAAAEOg9Vx5qbwc+KFQTVFGOPpNYFh9n7Px9G53oDwQ8Y4DGnzlC3mNFD6pmC0JV+0teMg==", "4321ffcf-4626-45da-98ff-7ccd9c71755f" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethodMerchants_MerchantId",
                schema: "dbo",
                table: "PaymentMethodMerchants",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethodMerchants_PaymentMethodId",
                schema: "dbo",
                table: "PaymentMethodMerchants",
                column: "PaymentMethodId");
        }
    }
}
