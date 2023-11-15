using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedMoveModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoveDetails_MoveDetails_MovedDetailId",
                table: "MoveDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MoveDetails_Subdivisions_SubdivisionId",
                table: "MoveDetails");

            migrationBuilder.DropTable(
                name: "CrossOperationStorages");

            migrationBuilder.DropTable(
                name: "FinishDetailStorages");

            migrationBuilder.DropTable(
                name: "CrossOperationStoragePlaces");

            migrationBuilder.DropTable(
                name: "FinishedDetailStoragePlaces");

            migrationBuilder.DropTable(
                name: "VirtualStorages");

            migrationBuilder.RenameColumn(
                name: "SubdivisionId",
                table: "MoveDetails",
                newName: "StorageId");

            migrationBuilder.RenameColumn(
                name: "MovedDetailId",
                table: "MoveDetails",
                newName: "PreviousMoveDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_MoveDetails_SubdivisionId",
                table: "MoveDetails",
                newName: "IX_MoveDetails_StorageId");

            migrationBuilder.RenameIndex(
                name: "IX_MoveDetails_MovedDetailId",
                table: "MoveDetails",
                newName: "IX_MoveDetails_PreviousMoveDetailId");

            migrationBuilder.AlterColumn<int>(
                name: "TechnologicalProcessItemId",
                table: "MoveDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "InitiatorId",
                table: "MoveDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NextMoveDetailId",
                table: "MoveDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "MoveDetails",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OperationGraphs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    GraphDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    FinishDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Note = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubdivisionId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    ProductDetailId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationGraphs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationGraphs_Details_ProductDetailId",
                        column: x => x.ProductDetailId,
                        principalTable: "Details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationGraphs_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationGraphs_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationGraphs_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPhysicalStorage = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SubdivisionId = table.Column<int>(type: "int", nullable: false),
                    FatherStorageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Storages_Storages_FatherStorageId",
                        column: x => x.FatherStorageId,
                        principalTable: "Storages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Storages_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OperationGraphDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlannedNumber = table.Column<int>(type: "int", nullable: false),
                    TotalPlannedNumber = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DetailId = table.Column<int>(type: "int", nullable: false),
                    OperationGraphId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationGraphDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationGraphDetails_Details_DetailId",
                        column: x => x.DetailId,
                        principalTable: "Details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationGraphDetails_OperationGraphs_OperationGraphId",
                        column: x => x.OperationGraphId,
                        principalTable: "OperationGraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OperationGraphGroups",
                columns: table => new
                {
                    OperationGraphMainId = table.Column<int>(type: "int", nullable: false),
                    OperationGraphNextId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationGraphGroups", x => new { x.OperationGraphNextId, x.OperationGraphMainId });
                    table.ForeignKey(
                        name: "FK_OperationGraphGroups_OperationGraphs_OperationGraphMainId",
                        column: x => x.OperationGraphMainId,
                        principalTable: "OperationGraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationGraphGroups_OperationGraphs_OperationGraphNextId",
                        column: x => x.OperationGraphNextId,
                        principalTable: "OperationGraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OperationGraphUsers",
                columns: table => new
                {
                    OperationGraphId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsReadonly = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationGraphUsers", x => new { x.OperationGraphId, x.UserId });
                    table.ForeignKey(
                        name: "FK_OperationGraphUsers_OperationGraphs_OperationGraphId",
                        column: x => x.OperationGraphId,
                        principalTable: "OperationGraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationGraphUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StoragePlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Shelf = table.Column<int>(type: "int", nullable: false),
                    Place = table.Column<int>(type: "int", nullable: false),
                    Cell = table.Column<int>(type: "int", nullable: true),
                    StorageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoragePlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoragePlaces_Storages_StorageId",
                        column: x => x.StorageId,
                        principalTable: "Storages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OperationGraphDetailGroups",
                columns: table => new
                {
                    OperationGraphMainDetailId = table.Column<int>(type: "int", nullable: false),
                    OperationGraphNextDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationGraphDetailGroups", x => new { x.OperationGraphNextDetailId, x.OperationGraphMainDetailId });
                    table.ForeignKey(
                        name: "FK_OperationGraphDetailGroups_OperationGraphDetails_OperationGr~",
                        column: x => x.OperationGraphMainDetailId,
                        principalTable: "OperationGraphDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationGraphDetailGroups_OperationGraphDetails_OperationG~1",
                        column: x => x.OperationGraphNextDetailId,
                        principalTable: "OperationGraphDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OperationGraphDetailItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FactCount = table.Column<int>(type: "int", nullable: false),
                    IsHaveDefective = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Note = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OperationGraphDetailId = table.Column<int>(type: "int", nullable: false),
                    TechnologicalProcessItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationGraphDetailItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationGraphDetailItems_OperationGraphDetails_OperationGra~",
                        column: x => x.OperationGraphDetailId,
                        principalTable: "OperationGraphDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationGraphDetailItems_TechnologicalProcessItems_Technolo~",
                        column: x => x.TechnologicalProcessItemId,
                        principalTable: "TechnologicalProcessItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StoragePlaceMoveDetails",
                columns: table => new
                {
                    StoragePlaceId = table.Column<int>(type: "int", nullable: false),
                    MoveDetailId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoragePlaceMoveDetails", x => new { x.MoveDetailId, x.StoragePlaceId });
                    table.ForeignKey(
                        name: "FK_StoragePlaceMoveDetails_MoveDetails_MoveDetailId",
                        column: x => x.MoveDetailId,
                        principalTable: "MoveDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoragePlaceMoveDetails_StoragePlaces_StoragePlaceId",
                        column: x => x.StoragePlaceId,
                        principalTable: "StoragePlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MoveDetails_InitiatorId",
                table: "MoveDetails",
                column: "InitiatorId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveDetails_NextMoveDetailId",
                table: "MoveDetails",
                column: "NextMoveDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphDetailGroups_OperationGraphMainDetailId",
                table: "OperationGraphDetailGroups",
                column: "OperationGraphMainDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphDetailItems_Id",
                table: "OperationGraphDetailItems",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphDetailItems_OperationGraphDetailId",
                table: "OperationGraphDetailItems",
                column: "OperationGraphDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphDetailItems_TechnologicalProcessItemId",
                table: "OperationGraphDetailItems",
                column: "TechnologicalProcessItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphDetails_DetailId",
                table: "OperationGraphDetails",
                column: "DetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphDetails_Id",
                table: "OperationGraphDetails",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphDetails_OperationGraphId",
                table: "OperationGraphDetails",
                column: "OperationGraphId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphGroups_OperationGraphMainId",
                table: "OperationGraphGroups",
                column: "OperationGraphMainId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphs_Id",
                table: "OperationGraphs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphs_OwnerId",
                table: "OperationGraphs",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphs_ProductDetailId",
                table: "OperationGraphs",
                column: "ProductDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphs_StatusId",
                table: "OperationGraphs",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphs_SubdivisionId",
                table: "OperationGraphs",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationGraphUsers_UserId",
                table: "OperationGraphUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoragePlaceMoveDetails_StoragePlaceId",
                table: "StoragePlaceMoveDetails",
                column: "StoragePlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_StoragePlaces_StorageId",
                table: "StoragePlaces",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_Storages_FatherStorageId",
                table: "Storages",
                column: "FatherStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_Storages_Id",
                table: "Storages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Storages_SubdivisionId",
                table: "Storages",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Storages_Title",
                table: "Storages",
                column: "Title",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MoveDetails_MoveDetails_NextMoveDetailId",
                table: "MoveDetails",
                column: "NextMoveDetailId",
                principalTable: "MoveDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MoveDetails_MoveDetails_PreviousMoveDetailId",
                table: "MoveDetails",
                column: "PreviousMoveDetailId",
                principalTable: "MoveDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MoveDetails_Storages_StorageId",
                table: "MoveDetails",
                column: "StorageId",
                principalTable: "Storages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MoveDetails_Users_InitiatorId",
                table: "MoveDetails",
                column: "InitiatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoveDetails_MoveDetails_NextMoveDetailId",
                table: "MoveDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MoveDetails_MoveDetails_PreviousMoveDetailId",
                table: "MoveDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MoveDetails_Storages_StorageId",
                table: "MoveDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MoveDetails_Users_InitiatorId",
                table: "MoveDetails");

            migrationBuilder.DropTable(
                name: "OperationGraphDetailGroups");

            migrationBuilder.DropTable(
                name: "OperationGraphDetailItems");

            migrationBuilder.DropTable(
                name: "OperationGraphGroups");

            migrationBuilder.DropTable(
                name: "OperationGraphUsers");

            migrationBuilder.DropTable(
                name: "StoragePlaceMoveDetails");

            migrationBuilder.DropTable(
                name: "OperationGraphDetails");

            migrationBuilder.DropTable(
                name: "StoragePlaces");

            migrationBuilder.DropTable(
                name: "OperationGraphs");

            migrationBuilder.DropTable(
                name: "Storages");

            migrationBuilder.DropIndex(
                name: "IX_MoveDetails_InitiatorId",
                table: "MoveDetails");

            migrationBuilder.DropIndex(
                name: "IX_MoveDetails_NextMoveDetailId",
                table: "MoveDetails");

            migrationBuilder.DropColumn(
                name: "InitiatorId",
                table: "MoveDetails");

            migrationBuilder.DropColumn(
                name: "NextMoveDetailId",
                table: "MoveDetails");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "MoveDetails");

            migrationBuilder.RenameColumn(
                name: "StorageId",
                table: "MoveDetails",
                newName: "SubdivisionId");

            migrationBuilder.RenameColumn(
                name: "PreviousMoveDetailId",
                table: "MoveDetails",
                newName: "MovedDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_MoveDetails_StorageId",
                table: "MoveDetails",
                newName: "IX_MoveDetails_SubdivisionId");

            migrationBuilder.RenameIndex(
                name: "IX_MoveDetails_PreviousMoveDetailId",
                table: "MoveDetails",
                newName: "IX_MoveDetails_MovedDetailId");

            migrationBuilder.AlterColumn<int>(
                name: "TechnologicalProcessItemId",
                table: "MoveDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CrossOperationStoragePlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BusyPercent = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Place = table.Column<int>(type: "int", nullable: false),
                    Row = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrossOperationStoragePlaces", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FinishedDetailStoragePlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Stillage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinishedDetailStoragePlaces", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VirtualStorages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SubdivisionId = table.Column<int>(type: "int", nullable: false),
                    TechnologicalProcessItemId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirtualStorages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VirtualStorages_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VirtualStorages_TechnologicalProcessItems_TechnologicalProce~",
                        column: x => x.TechnologicalProcessItemId,
                        principalTable: "TechnologicalProcessItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CrossOperationStorages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CrossOperationStoragePlaceId = table.Column<int>(type: "int", nullable: false),
                    VirtualStorageId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrossOperationStorages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrossOperationStorages_CrossOperationStoragePlaces_CrossOper~",
                        column: x => x.CrossOperationStoragePlaceId,
                        principalTable: "CrossOperationStoragePlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrossOperationStorages_VirtualStorages_VirtualStorageId",
                        column: x => x.VirtualStorageId,
                        principalTable: "VirtualStorages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FinishDetailStorages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FinishedDetailStoragePlaceId = table.Column<int>(type: "int", nullable: false),
                    VirtualStorageId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinishDetailStorages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinishDetailStorages_FinishedDetailStoragePlaces_FinishedDet~",
                        column: x => x.FinishedDetailStoragePlaceId,
                        principalTable: "FinishedDetailStoragePlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinishDetailStorages_VirtualStorages_VirtualStorageId",
                        column: x => x.VirtualStorageId,
                        principalTable: "VirtualStorages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CrossOperationStorages_CrossOperationStoragePlaceId",
                table: "CrossOperationStorages",
                column: "CrossOperationStoragePlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CrossOperationStorages_Id",
                table: "CrossOperationStorages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrossOperationStorages_VirtualStorageId",
                table: "CrossOperationStorages",
                column: "VirtualStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_FinishDetailStorages_FinishedDetailStoragePlaceId",
                table: "FinishDetailStorages",
                column: "FinishedDetailStoragePlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_FinishDetailStorages_Id",
                table: "FinishDetailStorages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinishDetailStorages_VirtualStorageId",
                table: "FinishDetailStorages",
                column: "VirtualStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualStorages_Id",
                table: "VirtualStorages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VirtualStorages_SubdivisionId",
                table: "VirtualStorages",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualStorages_TechnologicalProcessItemId",
                table: "VirtualStorages",
                column: "TechnologicalProcessItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoveDetails_MoveDetails_MovedDetailId",
                table: "MoveDetails",
                column: "MovedDetailId",
                principalTable: "MoveDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MoveDetails_Subdivisions_SubdivisionId",
                table: "MoveDetails",
                column: "SubdivisionId",
                principalTable: "Subdivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
