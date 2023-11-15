using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOnDeleteTypesInOperationModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphDetailGroups_OperationGraphDetails_OperationGr~",
                table: "OperationGraphDetailGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphDetailGroups_OperationGraphDetails_OperationG~1",
                table: "OperationGraphDetailGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphGroups_OperationGraphs_OperationGraphMainId",
                table: "OperationGraphGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphGroups_OperationGraphs_OperationGraphNextId",
                table: "OperationGraphGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphUsers_Users_UserId",
                table: "OperationGraphUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphDetailGroups_OperationGraphDetails_OperationGr~",
                table: "OperationGraphDetailGroups",
                column: "OperationGraphMainDetailId",
                principalTable: "OperationGraphDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphDetailGroups_OperationGraphDetails_OperationG~1",
                table: "OperationGraphDetailGroups",
                column: "OperationGraphNextDetailId",
                principalTable: "OperationGraphDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphGroups_OperationGraphs_OperationGraphMainId",
                table: "OperationGraphGroups",
                column: "OperationGraphMainId",
                principalTable: "OperationGraphs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphGroups_OperationGraphs_OperationGraphNextId",
                table: "OperationGraphGroups",
                column: "OperationGraphNextId",
                principalTable: "OperationGraphs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphUsers_Users_UserId",
                table: "OperationGraphUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphDetailGroups_OperationGraphDetails_OperationGr~",
                table: "OperationGraphDetailGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphDetailGroups_OperationGraphDetails_OperationG~1",
                table: "OperationGraphDetailGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphGroups_OperationGraphs_OperationGraphMainId",
                table: "OperationGraphGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphGroups_OperationGraphs_OperationGraphNextId",
                table: "OperationGraphGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphUsers_Users_UserId",
                table: "OperationGraphUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphDetailGroups_OperationGraphDetails_OperationGr~",
                table: "OperationGraphDetailGroups",
                column: "OperationGraphMainDetailId",
                principalTable: "OperationGraphDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphDetailGroups_OperationGraphDetails_OperationG~1",
                table: "OperationGraphDetailGroups",
                column: "OperationGraphNextDetailId",
                principalTable: "OperationGraphDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphGroups_OperationGraphs_OperationGraphMainId",
                table: "OperationGraphGroups",
                column: "OperationGraphMainId",
                principalTable: "OperationGraphs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphGroups_OperationGraphs_OperationGraphNextId",
                table: "OperationGraphGroups",
                column: "OperationGraphNextId",
                principalTable: "OperationGraphs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphUsers_Users_UserId",
                table: "OperationGraphUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
