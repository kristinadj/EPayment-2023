using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSP.WebApi.Migrations
{
    public partial class TypoExternalInvoiceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExtrenalInvoiceId",
                schema: "dbo",
                table: "Invoices",
                newName: "ExternalInvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_ExtrenalInvoiceId",
                schema: "dbo",
                table: "Invoices",
                newName: "IX_Invoices_ExternalInvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExternalInvoiceId",
                schema: "dbo",
                table: "Invoices",
                newName: "ExtrenalInvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_ExternalInvoiceId",
                schema: "dbo",
                table: "Invoices",
                newName: "IX_Invoices_ExtrenalInvoiceId");
        }
    }
}
