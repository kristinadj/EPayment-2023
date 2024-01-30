using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class CustomerUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cc1e5433-cf53-40d1-851e-e2102180eb55",
                columns: new[] { "Address", "ConcurrencyStamp", "Email", "PasswordHash", "SecurityStamp" },
                values: new object[] { "789 Ulica jorgovana", "74de82ef-3e4c-4073-8d48-94bbaa018e29", "johndoe@gmail.com", "AQAAAAEAACcQAAAAEMfZfoY8JKmhkwxAUqTGc6rN+5mPRBCre+tS0GdScZQkucHFqego2izK5mk9xH8OzQ==", "6234167b-1a89-4b03-a984-f8b8e56a9584" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff997333-0c10-4fef-9d07-d2599fca2795",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "968eff0a-8976-46f1-a289-72cf529a5d54", "AQAAAAEAACcQAAAAELwallhuaX7U5CZocgVfthvOKEvEMXQrHgo/TjqK5jnBpF5Joeo7HqtzvBo/s2ZXTw==", "a2ae7609-9774-4873-b709-6eac70e32f3e" });

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
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cc1e5433-cf53-40d1-851e-e2102180eb55",
                columns: new[] { "Address", "ConcurrencyStamp", "Email", "PasswordHash", "SecurityStamp" },
                values: new object[] { "789 Ulica jorgovana,", "40d0d67b-255c-4524-9f15-bfaa9e79b915", "johndoe@email.com", "AQAAAAEAACcQAAAAEE8+3TqN8KtvOQ+MvzloSW8L9EZ2UQZi2iNJf11RWuKsgAuM6ov1tHl+eSqAVWxJCw==", "995b1230-1745-4aaa-b69f-e2f5ec3efb1c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff997333-0c10-4fef-9d07-d2599fca2795",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "725c2c77-db4b-4c79-ae0d-ec9ab2c2f0f8", "AQAAAAEAACcQAAAAEGEVT8KJCuDA6P9xsrxUvNivPlNEkKbus70dUmCoBCQN60EG1qJdyVzOWweN7h6LrA==", "74e4aa14-d6dd-4166-b60b-d8b8e86963f3" });

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
