using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Postex.Payment.Infrastructure.Migrations
{
    public partial class changeGuidtolong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Payment");

            migrationBuilder.CreateTable(
                name: "PaymentRequests",
                schema: "Payment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SentOrderNumber = table.Column<long>(type: "bigint", nullable: false),
                    PaymentState = table.Column<int>(type: "int", nullable: false),
                    FailReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PaymentToken = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    ReturnUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CancelUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SaleReferenceId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ErrorGatewayCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemovedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentRequestRefunds",
                schema: "Payment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentRequestId = table.Column<long>(type: "bigint", nullable: false),
                    SentOrderNumber = table.Column<long>(type: "bigint", nullable: false),
                    RefundState = table.Column<int>(type: "int", nullable: false),
                    FailReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ErrorGatewayCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemovedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRequestRefunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentRequestRefunds_PaymentRequests_PaymentRequestId",
                        column: x => x.PaymentRequestId,
                        principalSchema: "Payment",
                        principalTable: "PaymentRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequestRefunds_PaymentRequestId",
                schema: "Payment",
                table: "PaymentRequestRefunds",
                column: "PaymentRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentRequestRefunds",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "PaymentRequests",
                schema: "Payment");
        }
    }
}
