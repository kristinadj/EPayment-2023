using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class IsCanceled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                schema: "dbo",
                table: "UserSubscriptionPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f7544ad6-8486-40aa-bed2-90e00d5add1b", "AQAAAAEAACcQAAAAEJhTGRpS3lPvgv6RsWlxxY/exmMnr5AGCIqL2FI61/yikHdJtTad7YBNsFE4x+d4qw==", "0b23d78c-b3d9-412a-8ccf-81b1986c99bf" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "61d66c5a-7ba1-413d-b430-9927d277277e", "AQAAAAEAACcQAAAAEFOsGTYtRwaXUcqC0AGmgxolFAHlU0MGeWhnsV8NniMwXCWvLGfK7IJ1CIOKGoQTbQ==", "fba7c3eb-6e45-41ee-a2a9-0531ea84ae31" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                schema: "dbo",
                table: "UserSubscriptionPlans");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d0d445dd-5924-4853-9acd-163cc18c8bae", "AQAAAAEAACcQAAAAEF/Kiu6JTGX6cVsqNP/SJwcs4NCKbUhhTCQxlndQiosXyn5PzGNzaF4fWnm1LbNy0Q==", "e0aa3349-10b2-4030-953c-be8ba7300159" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a4ceee2a-5010-48cf-84eb-35adaab1f11c", "AQAAAAEAACcQAAAAEKA5FXBFS2FFIRjGe/6ZylfmP3vQ0zb85XHClVKsDHAmVGYTOWHnKZl5sxwWm5956A==", "d9e03975-6277-45b8-8405-6e897394a842" });
        }
    }
}
