using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayPalPaymentService.WebApi.Migrations
{
    public partial class InvoiceFieldUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvocieId",
                schema: "dbo",
                table: "Invoices",
                newName: "InvoiceId");

            migrationBuilder.AddColumn<string>(
                name: "PayerId",
                schema: "dbo",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayerId",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "InvoiceId",
                schema: "dbo",
                table: "Invoices",
                newName: "InvocieId");
        }
    }
}
