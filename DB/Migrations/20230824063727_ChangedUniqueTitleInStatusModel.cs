using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUniqueTitleInStatusModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Statuses_Title",
                table: "Statuses");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Title",
                table: "Statuses",
                column: "Title");

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "TableName", "Title" },
                values: new object[,]
                {
                    { 12, "operGraph", "В разработке" },
                    { 13, "operGraph", "Активный" },
                    { 14, "operGraph", "Приостановлен" },
                    { 15, "operGraph", "Завершен" },
                    { 16, "operGraph", "Отменен" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Statuses_Title",
                table: "Statuses");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Title",
                table: "Statuses",
                column: "Title",
                unique: true);
        }
    }
}
