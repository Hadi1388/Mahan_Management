using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace installments_Payment.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class fix14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProcessStatus",
                table: "TreatmentProcesses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessStatus",
                table: "TreatmentProcesses");
        }
    }
}
