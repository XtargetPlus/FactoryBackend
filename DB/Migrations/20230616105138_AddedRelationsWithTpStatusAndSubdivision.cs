using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedRelationsWithTpStatusAndSubdivision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubdivisionId",
                table: "TechnologicalProcessStatuses",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "ManufacturingPriority",
                table: "TechnologicalProcesses",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessStatuses_SubdivisionId",
                table: "TechnologicalProcessStatuses",
                column: "SubdivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologicalProcessStatuses_Subdivisions_SubdivisionId",
                table: "TechnologicalProcessStatuses",
                column: "SubdivisionId",
                principalTable: "Subdivisions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnologicalProcessStatuses_Subdivisions_SubdivisionId",
                table: "TechnologicalProcessStatuses");

            migrationBuilder.DropIndex(
                name: "IX_TechnologicalProcessStatuses_SubdivisionId",
                table: "TechnologicalProcessStatuses");

            migrationBuilder.DropColumn(
                name: "SubdivisionId",
                table: "TechnologicalProcessStatuses");

            migrationBuilder.AlterColumn<byte>(
                name: "ManufacturingPriority",
                table: "TechnologicalProcesses",
                type: "tinyint unsigned",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint unsigned");
        }
    }
}
