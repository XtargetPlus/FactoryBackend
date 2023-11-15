using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOnDeleteInGraphModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphDetailItems_OperationGraphDetails_OperationGra~",
                table: "OperationGraphDetailItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphDetails_OperationGraphs_OperationGraphId",
                table: "OperationGraphDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphUsers_OperationGraphs_OperationGraphId",
                table: "OperationGraphUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphDetailItems_OperationGraphDetails_OperationGra~",
                table: "OperationGraphDetailItems",
                column: "OperationGraphDetailId",
                principalTable: "OperationGraphDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphDetails_OperationGraphs_OperationGraphId",
                table: "OperationGraphDetails",
                column: "OperationGraphId",
                principalTable: "OperationGraphs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphUsers_OperationGraphs_OperationGraphId",
                table: "OperationGraphUsers",
                column: "OperationGraphId",
                principalTable: "OperationGraphs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphDetailItems_OperationGraphDetails_OperationGra~",
                table: "OperationGraphDetailItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphDetails_OperationGraphs_OperationGraphId",
                table: "OperationGraphDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphUsers_OperationGraphs_OperationGraphId",
                table: "OperationGraphUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphDetailItems_OperationGraphDetails_OperationGra~",
                table: "OperationGraphDetailItems",
                column: "OperationGraphDetailId",
                principalTable: "OperationGraphDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphDetails_OperationGraphs_OperationGraphId",
                table: "OperationGraphDetails",
                column: "OperationGraphId",
                principalTable: "OperationGraphs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphUsers_OperationGraphs_OperationGraphId",
                table: "OperationGraphUsers",
                column: "OperationGraphId",
                principalTable: "OperationGraphs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
