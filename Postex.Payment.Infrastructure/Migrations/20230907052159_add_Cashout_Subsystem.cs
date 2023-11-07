using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Postex.Payment.Infrastructure.Migrations
{
    public partial class add_Cashout_Subsystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MobileNo",
                schema: "Payment",
                table: "PaymentRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CashoutBatchRequest",
                schema: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TotalAmount = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CashoutBatchRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashoutItemRequest",
                schema: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    IBanNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CashoutState = table.Column<int>(type: "int", nullable: false),
                    FailReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeAmount = table.Column<int>(type: "int", nullable: true),
                    DestinationFirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationLastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CashoutBatchRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_CashoutItemRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashoutItemRequest_CashoutBatchRequest_CashoutBatchRequestId",
                        column: x => x.CashoutBatchRequestId,
                        principalSchema: "Payment",
                        principalTable: "CashoutBatchRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashoutItemRequest_CashoutBatchRequestId",
                schema: "Payment",
                table: "CashoutItemRequest",
                column: "CashoutBatchRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashoutItemRequest",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "CashoutBatchRequest",
                schema: "Payment");

            migrationBuilder.DropColumn(
                name: "MobileNo",
                schema: "Payment",
                table: "PaymentRequests");
        }
    }
}
