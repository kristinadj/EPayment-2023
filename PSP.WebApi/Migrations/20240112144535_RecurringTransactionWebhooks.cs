using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSP.WebApi.Migrations
{
    public partial class RecurringTransactionWebhooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecurringTransactionFailureUrl",
                schema: "dbo",
                table: "SubscriptionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecurringTransactionSuccessUrl",
                schema: "dbo",
                table: "SubscriptionDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecurringTransactionFailureUrl",
                schema: "dbo",
                table: "SubscriptionDetails");

            migrationBuilder.DropColumn(
                name: "RecurringTransactionSuccessUrl",
                schema: "dbo",
                table: "SubscriptionDetails");
        }
    }
}
