using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class NormalizedEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cc1e5433-cf53-40d1-851e-e2102180eb55",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6806badc-a3c2-4664-8cc6-aeeba7477be7", "JOHNDOE@GMAIL.COM", "AQAAAAEAACcQAAAAEEtDx0MmgWc6MpjdVXf/1KUwvn9jJun8ik+MnFskt+GjH9SYge9LtUithhlTlSpXkA==", "5470f20c-04f4-4160-9084-72947ee6649d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff997333-0c10-4fef-9d07-d2599fca2795",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fd19d660-d08b-47e3-a2ec-f2bda30660c7", "WEBSHOPADMIN@LAWPUBLISHINGAGENCY.COM", "AQAAAAEAACcQAAAAEAUl1ybjt9z+J2hAhNxdJhyQ8vggvmHiJlPNFtlPdZA7Z4joTWwnmlzYFyPBQwY5oQ==", "2d05f8d8-4a3d-4ec1-873c-130d21978d10" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "BusinessCustomers",
                keyColumn: "BusinessCustomerId",
                keyValue: 1,
                column: "SecretKey",
                value: "Fy5Mib9nbUSKQCZYRPeWKA==");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Cards",
                keyColumn: "CardId",
                keyValue: 1,
                column: "PanNumber",
                value: "lCeUSOrMw0PwocsTtLCWKvE0C+tQi0H/fHA/8R4tGBE=");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cc1e5433-cf53-40d1-851e-e2102180eb55",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "74de82ef-3e4c-4073-8d48-94bbaa018e29", null, "AQAAAAEAACcQAAAAEMfZfoY8JKmhkwxAUqTGc6rN+5mPRBCre+tS0GdScZQkucHFqego2izK5mk9xH8OzQ==", "6234167b-1a89-4b03-a984-f8b8e56a9584" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff997333-0c10-4fef-9d07-d2599fca2795",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "968eff0a-8976-46f1-a289-72cf529a5d54", null, "AQAAAAEAACcQAAAAELwallhuaX7U5CZocgVfthvOKEvEMXQrHgo/TjqK5jnBpF5Joeo7HqtzvBo/s2ZXTw==", "a2ae7609-9774-4873-b709-6eac70e32f3e" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "BusinessCustomers",
                keyColumn: "BusinessCustomerId",
                keyValue: 1,
                column: "SecretKey",
                value: "Fy5Mib9nbUSKQCZYRPeWKA==");

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
