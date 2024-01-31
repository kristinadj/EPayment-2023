using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class PriceUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8b0a3aba-4e48-4782-94e6-3dc6cf9b53a1", "AQAAAAEAACcQAAAAEAvj8HKZg646+m49HAJKqhr+DroXRPY1rXABFaNLqOmit7BWwmOo+e0KUoM32yZfAQ==", "df2639f1-c0c6-4e8d-9a5e-3635a60c9750" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "da12e18e-e5b7-494a-ade2-c7074104103e", "AQAAAAEAACcQAAAAEFhZU1HeloHkpH2MiA0JY2B7Zxy+0uRJmEGQS3GXRAHoGJc/gU01iJ4EWn/L5Srb0g==", "9c31f6c3-2564-461f-a6ab-38c113546439" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 21,
                column: "Price",
                value: 0.10000000000000001);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ca9fde5b-40ea-4cb2-a1c0-ac2ef71fc505", "AQAAAAEAACcQAAAAEFIUkXmpLya6qJrFIjIoEHMhf4gnp4G+nOlinmdGRbdo74Uk0KTNX7EwrPvi8j82AQ==", "df1992d2-8180-4686-b13e-880174eb2369" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "710fc307-74a8-475a-a984-75d1a46c459e", "AQAAAAEAACcQAAAAEM1w+oAwUolMK1OVptsU3tpuHPlcRChGXGLEdoLCyzr9jpfbEWSnke6SEF2PIjQdIw==", "438eafcb-7631-40c7-a1a3-c90dcaa22fb6" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 21,
                column: "Price",
                value: 0.0050000000000000001);
        }
    }
}
