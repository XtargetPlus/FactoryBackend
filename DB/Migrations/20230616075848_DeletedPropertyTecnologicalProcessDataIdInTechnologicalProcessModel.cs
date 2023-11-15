using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class DeletedPropertyTecnologicalProcessDataIdInTechnologicalProcessModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TecnologicalProcessDataId",
                table: "TechnologicalProcesses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TecnologicalProcessDataId",
                table: "TechnologicalProcesses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
