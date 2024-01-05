using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayPalPaymentService.WebApi.Migrations
{
    public partial class Subscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayPalBillingPlanId",
                schema: "dbo",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayPalBillingPlanProductId",
                schema: "dbo",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceType",
                schema: "dbo",
                table: "Invoices",
                type: "nvarchar(24)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PayPalSubscriptionId",
                schema: "dbo",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                column: "Secret",
                value: "W0cagwccKGE0DM8pfTJbN3BwypRB1oCFOitaKMae5e5Y4m3jm706vL5UcmsfWKBg+unP8DjjYFt+JLwiVYCwpB//BCyezLBL+l5i3Z5FgOKqAey9gi1jZw+XSpjUMt+M");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayPalBillingPlanId",
                schema: "dbo",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "PayPalBillingPlanProductId",
                schema: "dbo",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "InvoiceType",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "PayPalSubscriptionId",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Merchants",
                keyColumn: "MerchantId",
                keyValue: 1,
                column: "Secret",
                value: "W0cagwccKGE0DM8pfTJbN3BwypRB1oCFOitaKMae5e5Y4m3jm706vL5UcmsfWKBg+unP8DjjYFt+JLwiVYCwpB//BCyezLBL+l5i3Z5FgOKqAey9gi1jZw+XSpjUMt+M");
        }
    }
}
