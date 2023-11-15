using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipmnetOperationTool1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentOperationTool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ToolId = table.Column<int>(type: "int", nullable: false),
                    EquipmentOperationId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FatherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentOperationTool", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentOperationTool_EquipmentOperationTool_FatherId",
                        column: x => x.FatherId,
                        principalTable: "EquipmentOperationTool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentOperationTool_FatherId",
                table: "EquipmentOperationTool",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentOperationTool_ToolId",
                table: "EquipmentOperationTool",
                column: "ToolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentOperationTool");
        }
    }
}
