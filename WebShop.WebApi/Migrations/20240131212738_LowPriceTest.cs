using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class LowPriceTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Items",
                columns: new[] { "ItemId", "CurrencyId", "Description", "MerchantId", "Name", "Price" },
                values: new object[] { 21, 2, "PTest - very low price", 2, "Test - very low price", 0.0050000000000000001 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 21);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dfb03970-ae8d-4d3b-9614-031b09d71e52", "AQAAAAEAACcQAAAAEKXqy13p4vCPsW5jo+tMCCfB2Nef40b8n7jE6BUb7ekYUDVvL5Ypzz4TF1YsKBu2tw==", "09c2d1fb-53c3-494d-9e30-1531b500505f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95bb7ada-6246-4cd1-ae5a-39a575b8a2fd", "AQAAAAEAACcQAAAAEOHukHds2F8xRo6ag1Br9IJyR/E0BxPVWeQ/G/LP+HPogcGlTSQHMnhKiKk/BFfTOw==", "c2f93720-51eb-44c2-950c-beab367ed754" });
        }
    }
}
