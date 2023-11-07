using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Postex.Payment.Infrastructure.Migrations
{
    public partial class add_Cashout_Subsystem_ver1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                schema: "Payment",
                table: "CashoutItemRequest",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                schema: "Payment",
                table: "CashoutItemRequest",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
