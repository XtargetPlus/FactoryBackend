using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNextAndPrefiousInMoveDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoveDetails_MoveDetails_NextMoveDetailId",
                table: "MoveDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MoveDetails_MoveDetails_PreviousMoveDetailId",
                table: "MoveDetails");

            migrationBuilder.DropIndex(
                name: "IX_MoveDetails_NextMoveDetailId",
                table: "MoveDetails");

            migrationBuilder.DropIndex(
                name: "IX_MoveDetails_PreviousMoveDetailId",
                table: "MoveDetails");

            migrationBuilder.DropColumn(
                name: "NextMoveDetailId",
                table: "MoveDetails");

            migrationBuilder.DropColumn(
                name: "PreviousMoveDetailId",
                table: "MoveDetails");

            migrationBuilder.CreateTable(
                name: "NextMovesDetails",
                columns: table => new
                {
                    CurrentMoveDetail = table.Column<int>(type: "int", nullable: false),
                    NextMoveDetail = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NextMovesDetails", x => new { x.CurrentMoveDetail, x.NextMoveDetail });
                    table.ForeignKey(
                        name: "FK_NextMovesDetails_MoveDetails_CurrentMoveDetail",
                        column: x => x.CurrentMoveDetail,
                        principalTable: "MoveDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NextMovesDetails_MoveDetails_NextMoveDetail",
                        column: x => x.NextMoveDetail,
                        principalTable: "MoveDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PreviousMovesDetails",
                columns: table => new
                {
                    CurrentMoveDetail = table.Column<int>(type: "int", nullable: false),
                    PreviousMoveDetail = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousMovesDetails", x => new { x.CurrentMoveDetail, x.PreviousMoveDetail });
                    table.ForeignKey(
                        name: "FK_PreviousMovesDetails_MoveDetails_CurrentMoveDetail",
                        column: x => x.CurrentMoveDetail,
                        principalTable: "MoveDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreviousMovesDetails_MoveDetails_PreviousMoveDetail",
                        column: x => x.PreviousMoveDetail,
                        principalTable: "MoveDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_NextMovesDetails_NextMoveDetail",
                table: "NextMovesDetails",
                column: "NextMoveDetail");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousMovesDetails_PreviousMoveDetail",
                table: "PreviousMovesDetails",
                column: "PreviousMoveDetail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NextMovesDetails");

            migrationBuilder.DropTable(
                name: "PreviousMovesDetails");

            migrationBuilder.AddColumn<int>(
                name: "NextMoveDetailId",
                table: "MoveDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousMoveDetailId",
                table: "MoveDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoveDetails_NextMoveDetailId",
                table: "MoveDetails",
                column: "NextMoveDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveDetails_PreviousMoveDetailId",
                table: "MoveDetails",
                column: "PreviousMoveDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoveDetails_MoveDetails_NextMoveDetailId",
                table: "MoveDetails",
                column: "NextMoveDetailId",
                principalTable: "MoveDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MoveDetails_MoveDetails_PreviousMoveDetailId",
                table: "MoveDetails",
                column: "PreviousMoveDetailId",
                principalTable: "MoveDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
