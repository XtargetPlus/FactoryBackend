using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedOperationGraphDetailItemIdInMoveDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OperationGraphDetailItemId",
                table: "MoveDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoveDetails_OperationGraphDetailItemId",
                table: "MoveDetails",
                column: "OperationGraphDetailItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoveDetails_OperationGraphDetailItems_OperationGraphDetailIt~",
                table: "MoveDetails",
                column: "OperationGraphDetailItemId",
                principalTable: "OperationGraphDetailItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoveDetails_OperationGraphDetailItems_OperationGraphDetailIt~",
                table: "MoveDetails");

            migrationBuilder.DropIndex(
                name: "IX_MoveDetails_OperationGraphDetailItemId",
                table: "MoveDetails");

            migrationBuilder.DropColumn(
                name: "OperationGraphDetailItemId",
                table: "MoveDetails");
        }
    }
}
