using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace installments_Payment.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class fix7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrepaymentReceiptPath",
                table: "Requests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrepaymentReceiptPath",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
