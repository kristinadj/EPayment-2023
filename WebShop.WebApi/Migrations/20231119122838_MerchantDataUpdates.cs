using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class MerchantDataUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "Address", "ConcurrencyStamp", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "123 Main Street", "6c4de0d5-96fe-4a82-940f-ea5071b1d544", "AQAAAAEAACcQAAAAEOg9Vx5qbwc+KFQTVFGOPpNYFh9n7Px9G53oDwQ8Y4DGnzlC3mNFD6pmC0JV+0teMg==", "+1 555-123-4567", "4321ffcf-4626-45da-98ff-7ccd9c71755f" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "Address", "ConcurrencyStamp", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { null, "737b9101-4845-4058-9d2d-6f5bcd646edb", "AQAAAAEAACcQAAAAEL9UIqlmb2lHgebPLrDz5E2XHZBlIQSwteh51UvwsKdBhVkm3Rm0/ikJSYj6XSdavg==", null, "427b3795-7fc2-434c-b6c0-007a11ee21fd" });
        }
    }
}
