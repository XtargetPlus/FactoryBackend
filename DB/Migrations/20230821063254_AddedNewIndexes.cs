using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "qwerty",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Subdivisions_Title",
                table: "Subdivisions",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Price",
                table: "Products",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_FullName",
                table: "Operations",
                column: "FullName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_ShortName",
                table: "Operations",
                column: "ShortName");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_Title",
                table: "Equipments",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentDetails_Title",
                table: "EquipmentDetails",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_DetailsChildren_Count",
                table: "DetailsChildren",
                column: "Count");

            migrationBuilder.CreateIndex(
                name: "IX_DetailsChildren_Number",
                table: "DetailsChildren",
                column: "Number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subdivisions_Title",
                table: "Subdivisions");

            migrationBuilder.DropIndex(
                name: "IX_Products_Price",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Operations_FullName",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Operations_ShortName",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_Title",
                table: "Equipments");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentDetails_Title",
                table: "EquipmentDetails");

            migrationBuilder.DropIndex(
                name: "IX_DetailsChildren_Count",
                table: "DetailsChildren");

            migrationBuilder.DropIndex(
                name: "IX_DetailsChildren_Number",
                table: "DetailsChildren");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "qwerty")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
