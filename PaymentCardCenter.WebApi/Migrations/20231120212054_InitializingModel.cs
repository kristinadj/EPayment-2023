using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentCardCenter.WebApi.Migrations
{
    public partial class InitializingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Banks",
                schema: "dbo",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardStartNumbers = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RedirectUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "dbo",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "dbo",
                columns: table => new
                {
                    Transactionid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AquirerBankId = table.Column<int>(type: "int", nullable: false),
                    AquirerTransctionId = table.Column<int>(type: "int", nullable: false),
                    AquirerTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssuerBankId = table.Column<int>(type: "int", nullable: true),
                    IssuerTransactionId = table.Column<int>(type: "int", nullable: true),
                    IssuerTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    TransactionStatus = table.Column<string>(type: "nvarchar(24)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Transactionid);
                    table.ForeignKey(
                        name: "FK_Transactions_Banks_AquirerBankId",
                        column: x => x.AquirerBankId,
                        principalSchema: "dbo",
                        principalTable: "Banks",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Banks_IssuerBankId",
                        column: x => x.IssuerBankId,
                        principalSchema: "dbo",
                        principalTable: "Banks",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "dbo",
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionLogs",
                schema: "dbo",
                columns: table => new
                {
                    TransactionLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    TransactionStatus = table.Column<string>(type: "nvarchar(24)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLogs", x => x.TransactionLogId);
                    table.ForeignKey(
                        name: "FK_TransactionLogs_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "dbo",
                        principalTable: "Transactions",
                        principalColumn: "Transactionid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Banks",
                columns: new[] { "BankId", "CardStartNumbers", "RedirectUrl" },
                values: new object[,]
                {
                    { 1, "1234", "https://localhost:7092/api" },
                    { 2, "2023", "https://localhost:7130/api" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Currencies",
                columns: new[] { "CurrencyId", "Code", "Name", "Symbol" },
                values: new object[,]
                {
                    { 1, "RSD", "Serbian Dinar", "RSD" },
                    { 2, "EUR", "Euro", "€" },
                    { 3, "USD", "American Dollar", "$" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banks_CardStartNumbers",
                schema: "dbo",
                table: "Banks",
                column: "CardStartNumbers",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Code",
                schema: "dbo",
                table: "Currencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_TransactionId",
                schema: "dbo",
                table: "TransactionLogs",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AquirerBankId",
                schema: "dbo",
                table: "Transactions",
                column: "AquirerBankId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CurrencyId",
                schema: "dbo",
                table: "Transactions",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_IssuerBankId",
                schema: "dbo",
                table: "Transactions",
                column: "IssuerBankId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Banks",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "dbo");
        }
    }
}
