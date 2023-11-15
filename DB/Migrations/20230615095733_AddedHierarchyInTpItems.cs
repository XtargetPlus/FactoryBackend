using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedHierarchyInTpItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainTechnologicalProcessItemId",
                table: "TechnologicalProcessItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessItems_MainTechnologicalProcessItemId",
                table: "TechnologicalProcessItems",
                column: "MainTechnologicalProcessItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologicalProcessItems_TechnologicalProcessItems_MainTech~",
                table: "TechnologicalProcessItems",
                column: "MainTechnologicalProcessItemId",
                principalTable: "TechnologicalProcessItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnologicalProcessItems_TechnologicalProcessItems_MainTech~",
                table: "TechnologicalProcessItems");

            migrationBuilder.DropIndex(
                name: "IX_TechnologicalProcessItems_MainTechnologicalProcessItemId",
                table: "TechnologicalProcessItems");

            migrationBuilder.DropColumn(
                name: "MainTechnologicalProcessItemId",
                table: "TechnologicalProcessItems");
        }
    }
}
