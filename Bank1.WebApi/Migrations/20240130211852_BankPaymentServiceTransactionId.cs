using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class BankPaymentServiceTransactionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BankPaymentServiceTransactionId",
                schema: "dbo",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 3,
                column: "AccountNumber",
                value: "105-0000000000001-26");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cc1e5433-cf53-40d1-851e-e2102180eb55",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "62e66885-3081-4a88-bef3-b87d3f561283", "AQAAAAEAACcQAAAAEHQhY3iNZMVIqO052n/DUKT4d9ti9HES6V3ZiX5U1YGjLxZi+x2VcM5UZrpoRVfT8A==", "9fcea6c5-a68e-476e-bafa-1ce31e96db39" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff997333-0c10-4fef-9d07-d2599fca2795",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "937939ae-4288-4434-9198-c97d5833ce88", "AQAAAAEAACcQAAAAEDHAlStHZ3B0XusjPNTHzaUBCw6EUNAKsT5xNSfpMVGwJnKJ1PJTzWgoHgBL74sr6w==", "fe7739bb-759c-4e37-959e-f59d5c7f9125" });

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
            migrationBuilder.DropColumn(
                name: "BankPaymentServiceTransactionId",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 3,
                column: "AccountNumber",
                value: "105-0000000000001-37");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cc1e5433-cf53-40d1-851e-e2102180eb55",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6806badc-a3c2-4664-8cc6-aeeba7477be7", "AQAAAAEAACcQAAAAEEtDx0MmgWc6MpjdVXf/1KUwvn9jJun8ik+MnFskt+GjH9SYge9LtUithhlTlSpXkA==", "5470f20c-04f4-4160-9084-72947ee6649d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff997333-0c10-4fef-9d07-d2599fca2795",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fd19d660-d08b-47e3-a2ec-f2bda30660c7", "AQAAAAEAACcQAAAAEAUl1ybjt9z+J2hAhNxdJhyQ8vggvmHiJlPNFtlPdZA7Z4joTWwnmlzYFyPBQwY5oQ==", "2d05f8d8-4a3d-4ec1-873c-130d21978d10" });

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
