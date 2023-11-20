using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank2.WebApi.Migrations
{
    public partial class PccUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "IssuerTimestamp",
                schema: "dbo",
                table: "Transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IssuerTransactionId",
                schema: "dbo",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IssuerTransactions",
                schema: "dbo",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuerAccountId = table.Column<int>(type: "int", nullable: false),
                    AquirerTransactionId = table.Column<int>(type: "int", nullable: false),
                    AquirerTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionStatus = table.Column<string>(type: "nvarchar(24)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuerTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_IssuerTransactions_Accounts_IssuerAccountId",
                        column: x => x.IssuerAccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssuerTransactions_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "dbo",
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssuerTransactions_CurrencyId",
                schema: "dbo",
                table: "IssuerTransactions",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuerTransactions_IssuerAccountId",
                schema: "dbo",
                table: "IssuerTransactions",
                column: "IssuerAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssuerTransactions",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "IssuerTimestamp",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "IssuerTransactionId",
                schema: "dbo",
                table: "Transactions");
        }
    }
}
