using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class PspMerchantId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PspMerchantId",
                schema: "dbo",
                table: "Merchants",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5193d1cf-43a3-4d37-ac8c-14237726748b", "AQAAAAEAACcQAAAAEHiL1hiAsqmiRA6wjPSR9qdFJZFOUtzakcKHM6EqOaYvHYx1U2m9/SnIfJvRXw8Nxw==", "e95c438c-984a-489c-85ed-1a87aa86890b" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PspMerchantId",
                schema: "dbo",
                table: "Merchants");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dd1a0171-a25b-4324-86c5-6f0864598499", "AQAAAAEAACcQAAAAECyRvUXrJ1eaJWOfTscS6QDx5QHl9yEFnb/AIScjT124kQXa21ZWja4v9Sd/653CRg==", "c85b5daf-017c-4ad6-956f-ae18889a17b0" });
        }
    }
}
