using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class RemovedInvoiceOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6954c8c9-27f5-47b0-abad-6677f5792eb5", "AQAAAAEAACcQAAAAEPREph/Kj5UYvmdiZ49LbHNeRiKxwC8GtGzXOcpOsIyhrFmVo/zkG/nYyfkYfa7LQQ==", "d2fbdf1b-b89d-45e0-816a-4dd84c5d5f64" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f055c66c-be5f-44e0-a3af-eae72813e08f", "AQAAAAEAACcQAAAAEBzAR0WNyCfWIVKg3bjWL4XytfYHOKCkFj3Dh9arOCbABzTQDHVIg6FKEZ5TCyvfvw==", "af72a6d3-1198-4bf0-90b7-188e824af1e5" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
