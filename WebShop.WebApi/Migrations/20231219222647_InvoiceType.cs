using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class InvoiceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceType",
                schema: "dbo",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "Invoices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d4b0bd63-7ff8-481f-b506-96012a0b61e4", "AQAAAAEAACcQAAAAEIErxEEkMV6ATloEmmwV/8FjYYOpurKBJmAMDdcfVoemg/S0jUqjWW/leQKnWtRXyw==", "d109ed56-13ea-4d4f-b29d-e805f1f3af8d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26b301ef-f544-4fe9-a864-b91b40ec928b", "AQAAAAEAACcQAAAAEMcj0G2wOlRgsJvPy3VBjXwrmIFBG6sAiqNDlQEX8pcqop7u7t4b+nBvlPWQCS9znQ==", "f7aa5fef-1f99-46f3-ab0d-c787fa0747fe" });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_UserId",
                schema: "dbo",
                table: "Invoices",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_AspNetUsers_UserId",
                schema: "dbo",
                table: "Invoices",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_AspNetUsers_UserId",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_UserId",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "InvoiceType",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "648899a7-fbd1-4a94-b127-2f981f66179d", "AQAAAAEAACcQAAAAEFyH/fsXqFhK0BRrPv+N4lpPdHhxuDtSLM3wFsKtPOExNTxKYrSmDkH8fWY+OOLoog==", "ce1263ae-278c-4f2d-bc08-5b2217832b15" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3b99c95f-861c-4109-8f85-e1bf7d6ea8d3", "AQAAAAEAACcQAAAAEKtUAcAcBr3xbhveaxSVkm8S0ZvaK2kcHLR72SzauOm9TRj8TXT3WISmW+dut/66cA==", "60800aa1-729d-47f1-82c7-986db280bac4" });
        }
    }
}
