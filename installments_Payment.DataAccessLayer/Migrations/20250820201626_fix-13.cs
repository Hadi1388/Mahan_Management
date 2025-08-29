using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace installments_Payment.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class fix13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentProcess_Requests_RequestId",
                table: "TreatmentProcess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreatmentProcess",
                table: "TreatmentProcess");

            migrationBuilder.RenameTable(
                name: "TreatmentProcess",
                newName: "TreatmentProcesses");

            migrationBuilder.RenameIndex(
                name: "IX_TreatmentProcess_RequestId",
                table: "TreatmentProcesses",
                newName: "IX_TreatmentProcesses_RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreatmentProcesses",
                table: "TreatmentProcesses",
                column: "TreatmentProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentProcesses_Requests_RequestId",
                table: "TreatmentProcesses",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentProcesses_Requests_RequestId",
                table: "TreatmentProcesses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreatmentProcesses",
                table: "TreatmentProcesses");

            migrationBuilder.RenameTable(
                name: "TreatmentProcesses",
                newName: "TreatmentProcess");

            migrationBuilder.RenameIndex(
                name: "IX_TreatmentProcesses_RequestId",
                table: "TreatmentProcess",
                newName: "IX_TreatmentProcess_RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreatmentProcess",
                table: "TreatmentProcess",
                column: "TreatmentProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentProcess_Requests_RequestId",
                table: "TreatmentProcess",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
