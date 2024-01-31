using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    public partial class BankModelUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1ed630d3-6e45-4b08-9a79-654d66550129", "AQAAAAEAACcQAAAAEFs5jPuhlCRyi4bfy+YJXzg6UkJadsfowXvAJAWxUibMld/WBWXaoeF/Eyrj5wlhyA==", "3064522d-c8fa-447c-904e-cbd419861677" });

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
                values: new object[] { "cd182e5b-f831-428a-b7f2-d41a5a581cee", "AQAAAAEAACcQAAAAECo9SRv/+iceg6Xp2VXutCR7B0rMWSypiYgbeh7WIlWjJy2iiMw/FKwP0kwZwlpIdQ==", "050842c8-c857-4807-9cca-fc203f6866b3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ff997333-0c10-4fef-9d07-d2599fca2795",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "98aed1c5-8b26-4e48-919f-31425ff8048b", "AQAAAAEAACcQAAAAEL40geoYRhsZMaOZSs9k9tlLwPx2EH641Yh6jG1ivgUa4wcvBJCn/SjYYuO1rPlnBQ==", "7bace500-77f1-41d0-abea-51a3af87b09e" });

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
