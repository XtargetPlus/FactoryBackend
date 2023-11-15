using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewRelationOperationGraphDetailModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TechnologicalProcessId",
                table: "OperationGraphDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphDetails_TechnologicalProcessId",
                table: "OperationGraphDetails",
                column: "TechnologicalProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphDetails_TechnologicalProcesses_TechnologicalPr~",
                table: "OperationGraphDetails",
                column: "TechnologicalProcessId",
                principalTable: "TechnologicalProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphDetails_TechnologicalProcesses_TechnologicalPr~",
                table: "OperationGraphDetails");

            migrationBuilder.DropIndex(
                name: "IX_OperationGraphDetails_TechnologicalProcessId",
                table: "OperationGraphDetails");

            migrationBuilder.DropColumn(
                name: "TechnologicalProcessId",
                table: "OperationGraphDetails");
        }
    }
}
