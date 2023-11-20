using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank2.WebApi.Migrations
{
    public partial class InitalizingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

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
                name: "Customers",
                schema: "dbo",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                schema: "dbo",
                columns: table => new
                {
                    ExchangeRateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromCurrencyId = table.Column<int>(type: "int", nullable: false),
                    ToCurrencyId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.ExchangeRateId);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_Currencies_FromCurrencyId",
                        column: x => x.FromCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_Currencies_ToCurrencyId",
                        column: x => x.ToCurrencyId,
                        principalSchema: "dbo",
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "dbo",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "dbo",
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_Customers_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "dbo",
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BusinessCustomers",
                schema: "dbo",
                columns: table => new
                {
                    BusinessCustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessCustomers", x => x.BusinessCustomerId);
                    table.ForeignKey(
                        name: "FK_BusinessCustomers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "dbo",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CardHolderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PanNumber = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false),
                    ExpiratoryDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CVV = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                    table.ForeignKey(
                        name: "FK_Cards_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "dbo",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderAccountId = table.Column<int>(type: "int", nullable: true),
                    ReceiverAccountId = table.Column<int>(type: "int", nullable: false),
                    TransactionStatus = table.Column<string>(type: "nvarchar(24)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionSuccessUrl = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    TransactionFailureUrl = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    TransactionErrorUrl = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_ReceiverAccountId",
                        column: x => x.ReceiverAccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_SenderAccountId",
                        column: x => x.SenderAccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
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
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "126 Palm Street", "admin@lpagency.com", "LP AGENCY", "", "+1 555-444-4567" },
                    { 2, "198 Oak Street,", "emilysmith@email.com", "Emily", "Smith", "+1 555-9324-7643" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Accounts",
                columns: new[] { "AccountId", "AccountNumber", "Balance", "CurrencyId", "OwnerId" },
                values: new object[,]
                {
                    { 1, "987612340", 14500.0, 1, 1 },
                    { 2, "1234098765", 12700.0, 1, 2 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "BusinessCustomers",
                columns: new[] { "BusinessCustomerId", "CustomerId", "Password" },
                values: new object[] { 1, 1, "LPAPassword123!" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "ExchangeRates",
                columns: new[] { "ExchangeRateId", "FromCurrencyId", "Rate", "ToCurrencyId" },
                values: new object[,]
                {
                    { 1, 1, 0.0085000000000000006, 2 },
                    { 2, 1, 0.0092999999999999992, 3 },
                    { 3, 2, 116.94, 1 },
                    { 4, 2, 1.0900000000000001, 3 },
                    { 5, 3, 107.53, 1 },
                    { 6, 3, 0.92000000000000004, 2 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Cards",
                columns: new[] { "CardId", "AccountId", "CVV", "CardHolderName", "ExpiratoryDate", "PanNumber" },
                values: new object[] { 1, 2, 955, "EMILY SMITH", "06/24", "2023 5678 9012 3456" });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountNumber",
                schema: "dbo",
                table: "Accounts",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CurrencyId",
                schema: "dbo",
                table: "Accounts",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_OwnerId",
                schema: "dbo",
                table: "Accounts",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCustomers_CustomerId",
                schema: "dbo",
                table: "BusinessCustomers",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_AccountId",
                schema: "dbo",
                table: "Cards",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Code",
                schema: "dbo",
                table: "Currencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_FromCurrencyId_ToCurrencyId",
                schema: "dbo",
                table: "ExchangeRates",
                columns: new[] { "FromCurrencyId", "ToCurrencyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_ToCurrencyId",
                schema: "dbo",
                table: "ExchangeRates",
                column: "ToCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_TransactionId",
                schema: "dbo",
                table: "TransactionLogs",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CurrencyId",
                schema: "dbo",
                table: "Transactions",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReceiverAccountId",
                schema: "dbo",
                table: "Transactions",
                column: "ReceiverAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SenderAccountId",
                schema: "dbo",
                table: "Transactions",
                column: "SenderAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessCustomers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Cards",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ExchangeRates",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TransactionLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "dbo");
        }
    }
}
