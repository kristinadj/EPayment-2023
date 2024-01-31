using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank2.WebApi.Migrations
{
    public partial class CardDataFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5250d188-dfd8-4bf4-8329-ee5c59807939",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f215552d-cced-4c02-9a75-26ba7a6512d4", "AQAAAAEAACcQAAAAEHY93jRvd/pYYlj0o5txTs7YKRGf5RwhY+XNbUWDPovz63L4YeR/Sg5som2eAfygUQ==", "73b5b2b0-bdcf-4894-b15e-2882c674ad7a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "84a03478-02d8-41a6-9397-735c6358470b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "448b1102-932f-4f5d-99fd-ee0c43584e57", "AQAAAAEAACcQAAAAEDxpN0rOrsSNgdcXcbWH3XbPYczTOywPQcFL6ShSk6ZZASy0dmqOUeYarFHB0NmMvQ==", "95a5e8d1-a5e7-4b08-8ef2-8806fa60251e" });

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
                columns: new[] { "CardHolderName", "PanNumber" },
                values: new object[] { "ANN SMITH", "7FHHbVIGQ88de2KUO5ZTXDDK4YcZ4UolwTTaj2LmyVc=" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5250d188-dfd8-4bf4-8329-ee5c59807939",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ce0734e1-fe27-406a-bcd5-810b39c96c24", "AQAAAAEAACcQAAAAEO9t7Pxbrhl2NdVcPMPx/MUDbTjVbn19nHe8TyjsrREwYFHnia2RZP8XRXdf4cXw7w==", "9ed2b207-16d0-459f-94fe-bb23e56000c5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "84a03478-02d8-41a6-9397-735c6358470b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "098953a4-a137-44fa-aef4-a2b57fe446b5", "AQAAAAEAACcQAAAAEP3R6C1KmQcqz3S9iTk0NyDdFSPY3EBqj/XsdaSHznUdue0IA/jbKZxxzQ6CAAewhQ==", "4796122d-218c-4bfe-af53-8e90a78301ae" });

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
                columns: new[] { "CardHolderName", "PanNumber" },
                values: new object[] { "JOHN DOE", "7FHHbVIGQ88de2KUO5ZTXL5eCPKQMiRuLusSNlLEF8o=" });
        }
    }
}
