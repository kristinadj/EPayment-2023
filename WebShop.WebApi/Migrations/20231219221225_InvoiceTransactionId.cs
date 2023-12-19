using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class InvoiceTransactionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                schema: "dbo",
                table: "Invoices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "478f0bd8-4a85-49c3-84a1-ef22339c9942", "AQAAAAEAACcQAAAAEAmkPlnsEdMrZ/qReCerRlQ9nxMGnvVFhl0fGzc0GxtZFmYKvy9CU5swrYf9khX6/w==", "12beb959-5229-4988-98f0-0dc38779cc48" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "64ad320f-970b-4bc5-8c46-ab91c7723c74", "AQAAAAEAACcQAAAAEMYxA7pG4G3crgMOlIiqtyovFIqsfqLsqbSOU+qbJffYEo1fl4tkKuN/1IbsFRrOhw==", "510fd157-3d91-4b13-b258-b54f7b64d173" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                schema: "dbo",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "de85c240-b017-41ed-9250-a6b835cd7b99", "AQAAAAEAACcQAAAAEO+0SJJW0DlUqMFXkca9ZCpjB+AXsaMyH7QcpeCck2oDAxARMDliXG60ugEQzm9jLA==", "cee113b7-6d51-4176-82a7-32dc78f7f17f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c729cf2b-f62e-4b9e-bfb1-74aa9abff603", "AQAAAAEAACcQAAAAEMFylar7A3Fajn7E0Hf7mjcohpyMuv7Bp2uv8aUyrpDJvTj1SDDVEeP+SErhYSSotQ==", "5e426440-3c62-4cea-ad49-443c972f26b6" });
        }
    }
}
