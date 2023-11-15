using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipmentOperationToolModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentOperationTool",
                columns: table => new
                {
                    ToolId = table.Column<int>(type: "int", nullable: false),
                    EquipmentOperationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentOperationTool", x => new { x.ToolId, x.EquipmentOperationId });
                    table.ForeignKey(
                        name: "FK_EquipmentOperationTool_EquipmentsOperations_EquipmentOperati~",
                        column: x => x.EquipmentOperationId,
                        principalTable: "EquipmentsOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentOperationTool_Tool_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentOperationTool_EquipmentOperationId",
                table: "EquipmentOperationTool",
                column: "EquipmentOperationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentOperationTool");
        }
    }
}
