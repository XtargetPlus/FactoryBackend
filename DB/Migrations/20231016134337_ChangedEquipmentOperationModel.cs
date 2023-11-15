using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ChangedEquipmentOperationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentsOperations");

            migrationBuilder.CreateTable(
                    name: "EquipmentsOperations",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "int", nullable: false)
                            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                        TechnologicalProcessItemId = table.Column<int>(type: "int", nullable: false),
                        EquipmentId = table.Column<int>(type: "int", nullable: false),
                        DebugTime = table.Column<float>(type: "float", nullable: false),
                        LeadTime = table.Column<float>(type: "float", nullable: false),
                        Priority = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                        Note = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_EquipmentsOperations", x => x.Id);
                        table.ForeignKey(
                            name: "FK_EquipmentsOperations_Equipments_EquipmentId",
                            column: x => x.EquipmentId,
                            principalTable: "Equipments",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                        table.ForeignKey(
                            name: "FK_EquipmentsOperations_TechnologicalProcessItems_Technological~",
                            column: x => x.TechnologicalProcessItemId,
                            principalTable: "TechnologicalProcessItems",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentsOperations_TechnologicalProcessItemId",
                table: "EquipmentsOperations",
                column: "TechnologicalProcessItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentsOperations_EquipmentId",
                table: "EquipmentsOperations",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentsOperations_Id",
                table: "EquipmentsOperations",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
