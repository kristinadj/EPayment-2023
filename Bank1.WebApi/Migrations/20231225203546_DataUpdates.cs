using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class DataUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                columns: new[] { "Address", "FirstName" },
                values: new object[] { "123 Glavna ulica", "Web prodavnica pravnog izdavaštva" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                column: "Address",
                value: "789 Ulica jorgovana,");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                columns: new[] { "Address", "FirstName" },
                values: new object[] { "123 Main Street", "Law Publishing Web Shop" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                column: "Address",
                value: "789 Elm Street,");
        }
    }
}
