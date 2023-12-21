using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankPaymentService.WebApi.Migrations
{
    public partial class InvoiceIdFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvocieId",
                schema: "dbo",
                table: "Invoices",
                newName: "InvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceId",
                schema: "dbo",
                table: "Invoices",
                newName: "InvocieId");
        }
    }
}
