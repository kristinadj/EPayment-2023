using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayPalPaymentService.WebApi.Migrations
{
    public partial class ReccurringPaymentColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RecurringPayment",
                schema: "dbo",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                name: "RecurringPayment",
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
