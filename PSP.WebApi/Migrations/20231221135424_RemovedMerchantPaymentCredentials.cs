using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSP.WebApi.Migrations
{
    public partial class RemovedMerchantPaymentCredentials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "dbo",
                table: "PaymentMethodMerchants");

            migrationBuilder.DropColumn(
                name: "Secret",
                schema: "dbo",
                table: "PaymentMethodMerchants");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
