using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class RecurringSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancellationId",
                schema: "dbo",
                table: "UserSubscriptionPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SupportsAutomaticPayments",
                schema: "dbo",
                table: "PaymentMethods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceType",
                schema: "dbo",
                table: "Invoices",
                type: "nvarchar(24)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7868e414-d3ce-436c-b4f0-c3f0221b9aee", "AQAAAAEAACcQAAAAEIOEuy3u3fO9m72n+PyGYTK57KT27jjvyTrdA1BSvWbU9Ex202g85nHVLHqV6FWe2g==", "b797495f-4e6e-46ac-a52d-88b5d0e6749b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8731f4a4-0379-4353-9854-b43a7bd3ef94", "AQAAAAEAACcQAAAAEJ+HKMcbnsVBUuBI/tMRdIJLRGzn1h0744mJgVkaf+2EV01AaGpEICQFOzC5m+Nuug==", "354bbbdc-602e-41da-878c-47c2eb84b6f0" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationId",
                schema: "dbo",
                table: "UserSubscriptionPlans");

            migrationBuilder.DropColumn(
                name: "SupportsAutomaticPayments",
                schema: "dbo",
                table: "PaymentMethods");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceType",
                schema: "dbo",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(24)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6954c8c9-27f5-47b0-abad-6677f5792eb5", "AQAAAAEAACcQAAAAEPREph/Kj5UYvmdiZ49LbHNeRiKxwC8GtGzXOcpOsIyhrFmVo/zkG/nYyfkYfa7LQQ==", "d2fbdf1b-b89d-45e0-816a-4dd84c5d5f64" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f055c66c-be5f-44e0-a3af-eae72813e08f", "AQAAAAEAACcQAAAAEBzAR0WNyCfWIVKg3bjWL4XytfYHOKCkFj3Dh9arOCbABzTQDHVIg6FKEZ5TCyvfvw==", "af72a6d3-1198-4bf0-90b7-188e824af1e5" });
        }
    }
}
