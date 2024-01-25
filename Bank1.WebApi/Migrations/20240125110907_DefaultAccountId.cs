using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class DefaultAccountId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultAccountId",
                schema: "dbo",
                table: "BusinessCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "BusinessCustomers",
                keyColumn: "BusinessCustomerId",
                keyValue: 1,
                columns: new[] { "DefaultAccountId", "Password" },
                values: new object[] { 1, "0hqo5asTUiTJe/2ZcF3vuA==" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "PanNumber",
                value: "lCeUSOrMw0PwocsTtLCWKvE0C+tQi0H/fHA/8R4tGBE=");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCustomers_DefaultAccountId",
                schema: "dbo",
                table: "BusinessCustomers",
                column: "DefaultAccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessCustomers_Accounts_DefaultAccountId",
                schema: "dbo",
                table: "BusinessCustomers",
                column: "DefaultAccountId",
                principalSchema: "dbo",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessCustomers_Accounts_DefaultAccountId",
                schema: "dbo",
                table: "BusinessCustomers");

            migrationBuilder.DropIndex(
                name: "IX_BusinessCustomers_DefaultAccountId",
                schema: "dbo",
                table: "BusinessCustomers");

            migrationBuilder.DropColumn(
                name: "DefaultAccountId",
                schema: "dbo",
                table: "BusinessCustomers");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "BusinessCustomers",
                keyColumn: "BusinessCustomerId",
                keyValue: 1,
                column: "Password",
                value: "0hqo5asTUiTJe/2ZcF3vuA==");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "PanNumber",
                value: "lCeUSOrMw0PwocsTtLCWKvE0C+tQi0H/fHA/8R4tGBE=");
        }
    }
}
