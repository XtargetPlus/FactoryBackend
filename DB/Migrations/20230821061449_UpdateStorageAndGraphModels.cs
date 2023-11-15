using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStorageAndGraphModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConfigrmingId",
                table: "OperationGraphs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountInStream",
                table: "OperationGraphDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DetailGraphNumberWithoutRepeats",
                table: "OperationGraphDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "OperationGraphDetails",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Usability",
                table: "OperationGraphDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsabilitySum",
                table: "OperationGraphDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OperationNumber",
                table: "OperationGraphDetailItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TechnologicalProcessItemId",
                table: "MoveDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsSuccess",
                table: "MoveDetails",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.CreateIndex(
                name: "IX_StoragePlaces_Id",
                table: "StoragePlaces",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphs_ConfigrmingId",
                table: "OperationGraphs",
                column: "ConfigrmingId");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationGraphs_Users_ConfigrmingId",
                table: "OperationGraphs",
                column: "ConfigrmingId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationGraphs_Users_ConfigrmingId",
                table: "OperationGraphs");

            migrationBuilder.DropIndex(
                name: "IX_StoragePlaces_Id",
                table: "StoragePlaces");

            migrationBuilder.DropIndex(
                name: "IX_OperationGraphs_ConfigrmingId",
                table: "OperationGraphs");

            migrationBuilder.DropColumn(
                name: "ConfigrmingId",
                table: "OperationGraphs");

            migrationBuilder.DropColumn(
                name: "CountInStream",
                table: "OperationGraphDetails");

            migrationBuilder.DropColumn(
                name: "DetailGraphNumberWithoutRepeats",
                table: "OperationGraphDetails");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "OperationGraphDetails");

            migrationBuilder.DropColumn(
                name: "Usability",
                table: "OperationGraphDetails");

            migrationBuilder.DropColumn(
                name: "UsabilitySum",
                table: "OperationGraphDetails");

            migrationBuilder.DropColumn(
                name: "OperationNumber",
                table: "OperationGraphDetailItems");

            migrationBuilder.AlterColumn<int>(
                name: "TechnologicalProcessItemId",
                table: "MoveDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSuccess",
                table: "MoveDetails",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);
        }
    }
}
