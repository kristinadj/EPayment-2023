using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class BankModelInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cc1e5433-cf53-40d1-851e-e2102180eb55",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "844fcdc6-7095-43a2-b5de-059ebc6e03da", "AQAAAAEAACcQAAAAEMZ93QN7DcmgJNX2exnKpz2tMpPxPFNz6NKPFn6JYv2Zhgkjun9TP8nxomu0IBpJiA==", "8a507cfd-670f-4b61-94af-04e54346ef6a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff997333-0c10-4fef-9d07-d2599fca2795",
                columns: new[] { "ConcurrencyStamp", "Email", "Name", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b5391014-4f4b-48ee-bc31-16ac5b390a01", "webshop1@gmail.com", "Web shop 1", "WEBSHOP1@GMAIL.COM", "AQAAAAEAACcQAAAAEMgPkZPy9MLcWVYTqJuoOQJ+jvYh+ESCMQFD185o9LBCkN6m/bxMa9iqVaBvv1pvKw==", "b5e8e06c-6856-41ec-a70e-c32f4f9a8a47" });

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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f6fabbbe-7339-4535-8e8d-7ef8c3e109e3", "AQAAAAEAACcQAAAAENP40DPvoBISbCZKaUn7RB2J0P4ZFbj2jzWPk9z/sS35YBgnaL085P1z8RET/r8ihw==", "1c4cd3ab-e0ec-42df-b1b3-31c6ca8e1892" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff997333-0c10-4fef-9d07-d2599fca2795",
                columns: new[] { "ConcurrencyStamp", "Email", "Name", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1ed630d3-6e45-4b08-9a79-654d66550129", "webshopadmin@lawpublishingagency.com", "Web prodavnica pravnog izdavaštva", "WEBSHOPADMIN@LAWPUBLISHINGAGENCY.COM", "AQAAAAEAACcQAAAAEFs5jPuhlCRyi4bfy+YJXzg6UkJadsfowXvAJAWxUibMld/WBWXaoeF/Eyrj5wlhyA==", "3064522d-c8fa-447c-904e-cbd419861677" });

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
