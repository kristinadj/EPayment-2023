using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class CvvEncrypt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "BusinessCustomers",
                keyColumn: "BusinessCustomerId",
                keyValue: 1,
                column: "Password",
                value: "0hqo5asTUiTJe/2ZcF3vuA==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "BusinessCustomers",
                keyColumn: "BusinessCustomerId",
                keyValue: 1,
                column: "Password",
                value: "0hqo5asTUiTJe/2ZcF3vuA==");
        }
    }
}
