using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace installments_Payment.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class fix11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "Installments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PaidByGateway",
                table: "Installments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Installments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PenaltyAmount",
                table: "Installments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiptImagePath",
                table: "Installments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiptNumber",
                table: "Installments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Installments");

            migrationBuilder.DropColumn(
                name: "PaidByGateway",
                table: "Installments");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Installments");

            migrationBuilder.DropColumn(
                name: "PenaltyAmount",
                table: "Installments");

            migrationBuilder.DropColumn(
                name: "ReceiptImagePath",
                table: "Installments");

            migrationBuilder.DropColumn(
                name: "ReceiptNumber",
                table: "Installments");
        }
    }
}
