using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class NullableFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSubscriptionPlans_InvoiceId",
                schema: "dbo",
                table: "UserSubscriptionPlans");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                schema: "dbo",
                table: "UserSubscriptionPlans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethodId",
                schema: "dbo",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "de85c240-b017-41ed-9250-a6b835cd7b99", "AQAAAAEAACcQAAAAEO+0SJJW0DlUqMFXkca9ZCpjB+AXsaMyH7QcpeCck2oDAxARMDliXG60ugEQzm9jLA==", "cee113b7-6d51-4176-82a7-32dc78f7f17f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c729cf2b-f62e-4b9e-bfb1-74aa9abff603", "AQAAAAEAACcQAAAAEMFylar7A3Fajn7E0Hf7mjcohpyMuv7Bp2uv8aUyrpDJvTj1SDDVEeP+SErhYSSotQ==", "5e426440-3c62-4cea-ad49-443c972f26b6" });

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptionPlans_InvoiceId",
                schema: "dbo",
                table: "UserSubscriptionPlans",
                column: "InvoiceId",
                unique: true,
                filter: "[InvoiceId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSubscriptionPlans_InvoiceId",
                schema: "dbo",
                table: "UserSubscriptionPlans");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                schema: "dbo",
                table: "UserSubscriptionPlans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethodId",
                schema: "dbo",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a90e7c1c-4581-48af-b096-245282ad1a0f", "AQAAAAEAACcQAAAAEFyD5Ie7BOJHmtZuOTT2NHo0MuRxzAdLBvMsFEhw6E7glFd8vpmqjw7EjwDIU2qQKQ==", "8531345f-4415-4b56-9eb6-9c190c9d709b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "83332538-26e3-4d0a-be5d-c8b57f185d0f", "AQAAAAEAACcQAAAAEIlGrjTH3B6pnIePoiHku23nSRkunVjNdqVVzaR1OFZljBIS8fLN4aF2xkSiXv3L/w==", "c6ede1ba-1622-4fe8-9cca-554f64775866" });

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptionPlans_InvoiceId",
                schema: "dbo",
                table: "UserSubscriptionPlans",
                column: "InvoiceId",
                unique: true);
        }
    }
}
