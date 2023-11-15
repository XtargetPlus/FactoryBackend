using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTechProcessStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TechnologicalProcessStatuses");

            migrationBuilder.CreateTable(
                    name: "TechnologicalProcessStatuses",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "int", nullable: false)
                            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                        TechnologicalProcessId = table.Column<int>(type: "int", nullable: false),
                        StatusId = table.Column<int>(type: "int", nullable: false),
                        UserId = table.Column<int>(type: "int", nullable: false),
                        StatusDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        Note = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_TechnologicalProcessStatuses", x => x.Id);
                        table.ForeignKey(
                            name: "FK_TechnologicalProcessStatuses_Statuses_StatusId",
                            column: x => x.StatusId,
                            principalTable: "Statuses",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                        table.ForeignKey(
                            name: "FK_TechnologicalProcessStatuses_TechnologicalProcesses_Technolo~",
                            column: x => x.TechnologicalProcessId,
                            principalTable: "TechnologicalProcesses",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                        table.ForeignKey(
                            name: "FK_TechnologicalProcessStatuses_Users_UserId",
                            column: x => x.UserId,
                            principalTable: "Users",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TechnologicalProcessStatuses",
                table: "TechnologicalProcessStatuses");

            migrationBuilder.DropIndex(
                name: "IX_TechnologicalProcessStatuses_Id",
                table: "TechnologicalProcessStatuses");

            migrationBuilder.DropIndex(
                name: "IX_TechnologicalProcessStatuses_TechnologicalProcessId",
                table: "TechnologicalProcessStatuses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TechnologicalProcessStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TechnologicalProcessStatuses",
                table: "TechnologicalProcessStatuses",
                columns: new[] { "TechnologicalProcessId", "UserId", "StatusId" });
        }
    }
}
