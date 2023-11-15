using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class dasfd : Migration
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

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessStatuses_Id",
                table: "TechnologicalProcessStatuses",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessStatuses_StatusId",
                table: "TechnologicalProcessStatuses",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessStatuses_UserId",
                table: "TechnologicalProcessStatuses",
                column: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "SubdivisionId",
                table: "TechnologicalProcessStatuses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessStatuses_SubdivisionId",
                table: "TechnologicalProcessStatuses",
                column: "SubdivisionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
