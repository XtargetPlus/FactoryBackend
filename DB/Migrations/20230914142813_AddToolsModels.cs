using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddToolsModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SerialNumber = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tool", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ToolParameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToolParameter_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentTool",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    ToolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTool", x => new { x.ToolId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_EquipmentTool_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentTool_Tool_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ToolCatalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FatherId = table.Column<int>(type: "int", nullable: true),
                    ToolId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolCatalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToolCatalog_ToolCatalog_FatherId",
                        column: x => x.FatherId,
                        principalTable: "ToolCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ToolCatalog_Tool_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tool",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ToolChild",
                columns: table => new
                {
                    FatherId = table.Column<int>(type: "int", nullable: false),
                    ChildId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolChild", x => new { x.FatherId, x.ChildId });
                    table.ForeignKey(
                        name: "FK_ToolChild_Tool_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Tool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ToolChild_Tool_FatherId",
                        column: x => x.FatherId,
                        principalTable: "Tool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ToolReplaceability",
                columns: table => new
                {
                    MasterId = table.Column<int>(type: "int", nullable: false),
                    SlaveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolReplaceability", x => new { x.MasterId, x.SlaveId });
                    table.ForeignKey(
                        name: "FK_ToolReplaceability_Tool_MasterId",
                        column: x => x.MasterId,
                        principalTable: "Tool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ToolReplaceability_Tool_SlaveId",
                        column: x => x.SlaveId,
                        principalTable: "Tool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ToolParameterConcrete",
                columns: table => new
                {
                    ToolId = table.Column<int>(type: "int", nullable: false),
                    ToolParameterId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolParameterConcrete", x => new { x.ToolParameterId, x.ToolId });
                    table.ForeignKey(
                        name: "FK_ToolParameterConcrete_ToolParameter_ToolParameterId",
                        column: x => x.ToolParameterId,
                        principalTable: "ToolParameter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ToolParameterConcrete_Tool_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ToolCatalogConcrete",
                columns: table => new
                {
                    ToolId = table.Column<int>(type: "int", nullable: false),
                    ToolCatalogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolCatalogConcrete", x => new { x.ToolId, x.ToolCatalogId });
                    table.ForeignKey(
                        name: "FK_ToolCatalogConcrete_ToolCatalog_ToolCatalogId",
                        column: x => x.ToolCatalogId,
                        principalTable: "ToolCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToolCatalogConcrete_Tool_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTool_EquipmentId",
                table: "EquipmentTool",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tool_Id",
                table: "Tool",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToolCatalog_FatherId",
                table: "ToolCatalog",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolCatalog_Id",
                table: "ToolCatalog",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToolCatalog_Title",
                table: "ToolCatalog",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToolCatalog_ToolId",
                table: "ToolCatalog",
                column: "ToolId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolCatalogConcrete_ToolCatalogId",
                table: "ToolCatalogConcrete",
                column: "ToolCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolChild_ChildId",
                table: "ToolChild",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolChild_Count",
                table: "ToolChild",
                column: "Count");

            migrationBuilder.CreateIndex(
                name: "IX_ToolChild_Priority",
                table: "ToolChild",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_ToolParameter_Id",
                table: "ToolParameter",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToolParameter_Title",
                table: "ToolParameter",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToolParameter_UnitId",
                table: "ToolParameter",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolParameterConcrete_ToolId",
                table: "ToolParameterConcrete",
                column: "ToolId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolReplaceability_SlaveId",
                table: "ToolReplaceability",
                column: "SlaveId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentTool");

            migrationBuilder.DropTable(
                name: "ToolCatalogConcrete");

            migrationBuilder.DropTable(
                name: "ToolChild");

            migrationBuilder.DropTable(
                name: "ToolParameterConcrete");

            migrationBuilder.DropTable(
                name: "ToolReplaceability");

            migrationBuilder.DropTable(
                name: "ToolCatalog");

            migrationBuilder.DropTable(
                name: "ToolParameter");

            migrationBuilder.DropTable(
                name: "Tool");
        }
    }
}
