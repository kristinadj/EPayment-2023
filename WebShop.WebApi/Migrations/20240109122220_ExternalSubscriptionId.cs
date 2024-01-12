using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class ExternalSubscriptionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CancellationId",
                schema: "dbo",
                table: "UserSubscriptionPlans",
                newName: "ExternalSubscriptionId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExternalSubscriptionId",
                schema: "dbo",
                table: "UserSubscriptionPlans",
                newName: "CancellationId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7868e414-d3ce-436c-b4f0-c3f0221b9aee", "AQAAAAEAACcQAAAAEIOEuy3u3fO9m72n+PyGYTK57KT27jjvyTrdA1BSvWbU9Ex202g85nHVLHqV6FWe2g==", "b797495f-4e6e-46ac-a52d-88b5d0e6749b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8731f4a4-0379-4353-9854-b43a7bd3ef94", "AQAAAAEAACcQAAAAEJ+HKMcbnsVBUuBI/tMRdIJLRGzn1h0744mJgVkaf+2EV01AaGpEICQFOzC5m+Nuug==", "354bbbdc-602e-41da-878c-47c2eb84b6f0" });
        }
    }
}
