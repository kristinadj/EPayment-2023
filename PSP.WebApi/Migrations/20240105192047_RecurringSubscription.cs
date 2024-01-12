using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSP.WebApi.Migrations
{
    public partial class RecurringSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SupportsAutomaticPayments",
                schema: "dbo",
                table: "PaymentMethods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceType",
                schema: "dbo",
                table: "Invoices",
                type: "nvarchar(24)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "RecurringPayment",
                schema: "dbo",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SubscriptionDetails",
                schema: "dbo",
                columns: table => new
                {
                    SubscriptionDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    SubscriberName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriberEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCategory = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionDetails", x => x.SubscriptionDetailsId);
                    table.ForeignKey(
                        name: "FK_SubscriptionDetails_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "dbo",
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionDetails_InvoiceId",
                schema: "dbo",
                table: "SubscriptionDetails",
                column: "InvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionDetails",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "SupportsAutomaticPayments",
                schema: "dbo",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "InvoiceType",
                schema: "dbo",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RecurringPayment",
                schema: "dbo",
                table: "Invoices");
        }
    }
}
