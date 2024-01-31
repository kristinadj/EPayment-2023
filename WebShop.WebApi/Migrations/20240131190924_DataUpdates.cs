using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    public partial class DataUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dfb03970-ae8d-4d3b-9614-031b09d71e52", "webshop2@gmail.com", "WEBSHOP2@GMAIL.COM", "AQAAAAEAACcQAAAAEKXqy13p4vCPsW5jo+tMCCfB2Nef40b8n7jE6BUb7ekYUDVvL5Ypzz4TF1YsKBu2tw==", "09c2d1fb-53c3-494d-9e30-1531b500505f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95bb7ada-6246-4cd1-ae5a-39a575b8a2fd", "webshop1@gmail.com", "WEBSHOP1@GMAIL.COM", "AQAAAAEAACcQAAAAEOHukHds2F8xRo6ag1Br9IJyR/E0BxPVWeQ/G/LP+HPogcGlTSQHMnhKiKk/BFfTOw==", "c2f93720-51eb-44c2-950c-beab367ed754" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e87d106-2e43-4a19-bd4c-843920dcf3e9",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "106f44c6-caa8-48e1-a46d-5f0454176102", "agencyadmin@legaldocsagency.com", "AGENCYADMIN@LEGALDOCSAGENCY.COM", "AQAAAAEAACcQAAAAEB2zRQIwaOKBjmIC3Pf+x1bk/Uc1KrAd91LTugFDfu59o0r8RZHpqlVta1gysti+/Q==", "f59f187c-08d6-49f2-b5b8-295e3d93782a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408b89e8-e8e5-4b97-9c88-f19593d66378",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f102c5c3-6d0b-49f1-a339-3a508c8c2000", "webshopadmin@lawpublishingagency.com", "WEBSHOPADMIN@LAWPUBLISHINGAGENCY.COM", "AQAAAAEAACcQAAAAEG/YfOnF/jbfI50whU62BoICZACCcT7JSgKXeyShoHxTMhgTT6eCwBzprP7v+zBttw==", "dd853222-b31f-4b04-8d1f-bb64564b50ae" });
        }
    }
}
