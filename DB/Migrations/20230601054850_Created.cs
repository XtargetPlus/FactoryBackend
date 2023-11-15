using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class Created : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessoryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BlankTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlankTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CrossOperationStoragePlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Place = table.Column<int>(type: "int", nullable: false),
                    BusyPercent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrossOperationStoragePlaces", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DetailTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SerialNumber = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentDetails", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentFailures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentFailures", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentParams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentParams", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FinishedDetailStoragePlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Stillage = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinishedDetailStoragePlaces", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MoveTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShortName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OutsideOrganizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutsideOrganizations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Professions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TableName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Subdivisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FatherId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subdivisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subdivisions_Subdivisions_FatherId",
                        column: x => x.FatherId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserForms", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WorkingParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    WorkingTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingParts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WorkingPart = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Failure = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DailyTask = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EquipmentDetail = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PurchaseDetail = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FinishDate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentStatuses_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SerialNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubdivisionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipments_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FathersName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FFL = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfessionNumber = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubdivisionId = table.Column<int>(type: "int", nullable: false),
                    ProfessionId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SerialNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Weight = table.Column<float>(type: "float", nullable: false),
                    DetailTypeId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Details_DetailTypes_DetailTypeId",
                        column: x => x.DetailTypeId,
                        principalTable: "DetailTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Details_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RoleClients",
                columns: table => new
                {
                    UserFormId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Add = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Edit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Delete = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Browsing = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClients", x => new { x.RoleId, x.UserFormId });
                    table.ForeignKey(
                        name: "FK_RoleClients_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleClients_UserForms_UserFormId",
                        column: x => x.UserFormId,
                        principalTable: "UserForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentDetailContents",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    EquipmentDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentDetailContents", x => new { x.EquipmentId, x.EquipmentDetailId });
                    table.ForeignKey(
                        name: "FK_EquipmentDetailContents_EquipmentDetails_EquipmentDetailId",
                        column: x => x.EquipmentDetailId,
                        principalTable: "EquipmentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentDetailContents_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentParamValues",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    EquipmentParamId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(254)", maxLength: 254, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentParamValues", x => new { x.EquipmentId, x.EquipmentParamId });
                    table.ForeignKey(
                        name: "FK_EquipmentParamValues_EquipmentParams_EquipmentParamId",
                        column: x => x.EquipmentParamId,
                        principalTable: "EquipmentParams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentParamValues_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentSchedules",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    WorkingPartId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FinishDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CanDebug = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentSchedules", x => new { x.EquipmentId, x.WorkingPartId });
                    table.ForeignKey(
                        name: "FK_EquipmentSchedules_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentSchedules_WorkingParts_WorkingPartId",
                        column: x => x.WorkingPartId,
                        principalTable: "WorkingParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentStatusUsers",
                columns: table => new
                {
                    EquipmentStatusId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentStatusUsers", x => new { x.UserId, x.EquipmentStatusId });
                    table.ForeignKey(
                        name: "FK_EquipmentStatusUsers_EquipmentStatuses_EquipmentStatusId",
                        column: x => x.EquipmentStatusId,
                        principalTable: "EquipmentStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentStatusUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentStatusValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Note = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPossibleToPlan = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EquipmentStatusId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    WorkingPartId = table.Column<int>(type: "int", nullable: true),
                    EquipmentFaulureId = table.Column<int>(type: "int", nullable: true),
                    DailyTaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentStatusValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentStatusValues_EquipmentFailures_EquipmentFaulureId",
                        column: x => x.EquipmentFaulureId,
                        principalTable: "EquipmentFailures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentStatusValues_EquipmentStatuses_EquipmentStatusId",
                        column: x => x.EquipmentStatusId,
                        principalTable: "EquipmentStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentStatusValues_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentStatusValues_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentStatusValues_WorkingParts_WorkingPartId",
                        column: x => x.WorkingPartId,
                        principalTable: "WorkingParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DetailReplaceabilities",
                columns: table => new
                {
                    FirstDetailId = table.Column<int>(type: "int", nullable: false),
                    SecondDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailReplaceabilities", x => new { x.FirstDetailId, x.SecondDetailId });
                    table.ForeignKey(
                        name: "FK_DetailReplaceabilities_Details_FirstDetailId",
                        column: x => x.FirstDetailId,
                        principalTable: "Details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetailReplaceabilities_Details_SecondDetailId",
                        column: x => x.SecondDetailId,
                        principalTable: "Details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DetailsChildren",
                columns: table => new
                {
                    ChildId = table.Column<int>(type: "int", nullable: false),
                    FatherId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<float>(type: "float", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailsChildren", x => new { x.FatherId, x.ChildId });
                    table.ForeignKey(
                        name: "FK_DetailsChildren_Details_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetailsChildren_Details_FatherId",
                        column: x => x.FatherId,
                        principalTable: "Details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Price = table.Column<float>(type: "float", nullable: false),
                    DetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Details_DetailId",
                        column: x => x.DetailId,
                        principalTable: "Details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TechnologicalProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FinishDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DevelopmentPriority = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    IsActual = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Note = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DetailId = table.Column<int>(type: "int", nullable: false),
                    DeveloperId = table.Column<int>(type: "int", nullable: false),
                    TecnologicalProcessDataId = table.Column<int>(type: "int", nullable: false),
                    ManufacturingPriority = table.Column<byte>(type: "tinyint unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologicalProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnologicalProcesses_Details_DetailId",
                        column: x => x.DetailId,
                        principalTable: "Details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnologicalProcesses_Users_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentDetailReplacements",
                columns: table => new
                {
                    EquipmentDetailId = table.Column<int>(type: "int", nullable: false),
                    EquipmentStatusValueId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentDetailReplacements", x => new { x.EquipmentDetailId, x.EquipmentStatusValueId });
                    table.ForeignKey(
                        name: "FK_EquipmentDetailReplacements_EquipmentDetails_EquipmentDetail~",
                        column: x => x.EquipmentDetailId,
                        principalTable: "EquipmentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentDetailReplacements_EquipmentStatusValues_EquipmentS~",
                        column: x => x.EquipmentStatusValueId,
                        principalTable: "EquipmentStatusValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClientProducts",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProducts", x => new { x.ProductId, x.ClientId });
                    table.ForeignKey(
                        name: "FK_ClientProducts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TechnologicalProcessData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Rate = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TecnologicalProcessId = table.Column<int>(type: "int", nullable: false),
                    BlankTypeId = table.Column<int>(type: "int", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologicalProcessData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnologicalProcessData_BlankTypes_BlankTypeId",
                        column: x => x.BlankTypeId,
                        principalTable: "BlankTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnologicalProcessData_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnologicalProcessData_TechnologicalProcesses_Tecnological~",
                        column: x => x.TecnologicalProcessId,
                        principalTable: "TechnologicalProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TechnologicalProcessItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<int>(type: "int", nullable: false),
                    OperationNumber = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Priority = table.Column<uint>(type: "int unsigned", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Show = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Note = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TechnologicalProcessId = table.Column<int>(type: "int", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologicalProcessItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnologicalProcessItems_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnologicalProcessItems_TechnologicalProcesses_Technologic~",
                        column: x => x.TechnologicalProcessId,
                        principalTable: "TechnologicalProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TechnologicalProcessStatuses",
                columns: table => new
                {
                    TechnologicalProcessId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StatusDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Note = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologicalProcessStatuses", x => new { x.TechnologicalProcessId, x.UserId, x.StatusId });
                    table.ForeignKey(
                        name: "FK_TechnologicalProcessStatuses_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnologicalProcessStatuses_TechnologicalProcesses_Technolo~",
                        column: x => x.TechnologicalProcessId,
                        principalTable: "TechnologicalProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnologicalProcessStatuses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Accessories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TaskDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FinishDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TaskNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Priority = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Note = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TechnologicalProcessItemId = table.Column<int>(type: "int", nullable: false),
                    AccessoryTypeId = table.Column<int>(type: "int", nullable: false),
                    DeveloperId = table.Column<int>(type: "int", nullable: false),
                    OutsideOrganizationId = table.Column<int>(type: "int", nullable: true),
                    DetailId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accessories_AccessoryTypes_AccessoryTypeId",
                        column: x => x.AccessoryTypeId,
                        principalTable: "AccessoryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accessories_Details_DetailId",
                        column: x => x.DetailId,
                        principalTable: "Details",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Accessories_OutsideOrganizations_OutsideOrganizationId",
                        column: x => x.OutsideOrganizationId,
                        principalTable: "OutsideOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accessories_TechnologicalProcessItems_TechnologicalProcessIt~",
                        column: x => x.TechnologicalProcessItemId,
                        principalTable: "TechnologicalProcessItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accessories_Users_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentPlans",
                columns: table => new
                {
                    TechnologicalProcessItemId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    PlainningData = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    WorkingKind = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentPlans", x => new { x.EquipmentId, x.TechnologicalProcessItemId });
                    table.ForeignKey(
                        name: "FK_EquipmentPlans_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentPlans_TechnologicalProcessItems_TechnologicalProces~",
                        column: x => x.TechnologicalProcessItemId,
                        principalTable: "TechnologicalProcessItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentsOperations",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    TechnologicalProcessItemId = table.Column<int>(type: "int", nullable: false),
                    DebugTime = table.Column<float>(type: "float", nullable: false),
                    LeadTime = table.Column<float>(type: "float", nullable: false),
                    Priority = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Note = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentsOperations", x => new { x.EquipmentId, x.TechnologicalProcessItemId });
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

            migrationBuilder.CreateTable(
                name: "MoveDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MoveDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    IsSuccess = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SubdivisionId = table.Column<int>(type: "int", nullable: false),
                    TechnologicalProcessItemId = table.Column<int>(type: "int", nullable: false),
                    MoveTypeId = table.Column<int>(type: "int", nullable: false),
                    MovedDetailId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoveDetails_MoveDetails_MovedDetailId",
                        column: x => x.MovedDetailId,
                        principalTable: "MoveDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MoveDetails_MoveTypes_MoveTypeId",
                        column: x => x.MoveTypeId,
                        principalTable: "MoveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoveDetails_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoveDetails_TechnologicalProcessItems_TechnologicalProcessIt~",
                        column: x => x.TechnologicalProcessItemId,
                        principalTable: "TechnologicalProcessItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VirtualStorages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Count = table.Column<int>(type: "int", nullable: false),
                    TechnologicalProcessItemId = table.Column<int>(type: "int", nullable: false),
                    SubdivisionId = table.Column<int>(type: "int", nullable: false)
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
                name: "AccessoryDevelopmentStatuses",
                columns: table => new
                {
                    AccessoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    StatusDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DocumentNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryDevelopmentStatuses", x => new { x.UserId, x.StatusId, x.AccessoryId });
                    table.ForeignKey(
                        name: "FK_AccessoryDevelopmentStatuses_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessoryDevelopmentStatuses_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessoryDevelopmentStatuses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessoryEquipments",
                columns: table => new
                {
                    AccessoryId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryEquipments", x => new { x.EquipmentId, x.AccessoryId });
                    table.ForeignKey(
                        name: "FK_AccessoryEquipments_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessoryEquipments_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
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
                    Count = table.Column<int>(type: "int", nullable: false),
                    CrossOperationStoragePlaceId = table.Column<int>(type: "int", nullable: false),
                    VirtualStorageId = table.Column<int>(type: "int", nullable: false)
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
                    Count = table.Column<int>(type: "int", nullable: false),
                    FinishedDetailStoragePlaceId = table.Column<int>(type: "int", nullable: false),
                    VirtualStorageId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.InsertData(
                table: "Professions",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Инженер-программист" },
                    { 2, "Инженер-технолог 3 категории" },
                    { 3, "Инженер-технолог 2 категории" },
                    { 4, "Инженер-технолог 1 категории" },
                    { 5, "Техник" },
                    { 6, "Ведущий инженер-технолог" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "PDO" },
                    { 3, "DirectorPDO" },
                    { 4, "LeadPDO" },
                    { 5, "DirectorOK" },
                    { 6, "OK" },
                    { 7, "technologistDeveloper" }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "TableName", "Title" },
                values: new object[,]
                {
                    { 1, "users", "Работает" },
                    { 2, "tp", "Не в работе" },
                    { 3, "users", "Уволен" },
                    { 4, "tp", "В работе" },
                    { 5, "tp", "Приостановлен" },
                    { 6, "tp", "На согласовании" },
                    { 7, "tp", "Возврат на доработку" },
                    { 8, "tp", "На выдачу" },
                    { 9, "tp", "Выдан" },
                    { 10, "tp", "Выдан дубликат" }
                });

            migrationBuilder.InsertData(
                table: "Subdivisions",
                columns: new[] { "Id", "FatherId", "Title" },
                values: new object[] { 1, null, "15 Отдел разработки программного обеспечения" });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Title" },
                values: new object[] { 1, "шт" });

            migrationBuilder.InsertData(
                table: "UserForms",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "role" },
                    { 2, "detail_structure" },
                    { 3, "detail" },
                    { 4, "detail-type" },
                    { 5, "detail_swap" },
                    { 6, "user" },
                    { 7, "profession" },
                    { 8, "subdivision" },
                    { 9, "user-form" },
                    { 10, "role_client" },
                    { 11, "product" },
                    { 12, "move-type" },
                    { 13, "status" },
                    { 14, "equipment-failure" },
                    { 15, "operation" },
                    { 16, "blank-type" }
                });

            migrationBuilder.InsertData(
                table: "RoleClients",
                columns: new[] { "RoleId", "UserFormId", "Add", "Browsing", "Delete", "Edit" },
                values: new object[,]
                {
                    { 1, 1, true, true, true, true },
                    { 1, 9, true, true, true, true },
                    { 1, 10, true, true, true, true }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FFL", "FathersName", "FirstName", "LastName", "Password", "ProfessionId", "ProfessionNumber", "RoleId", "StatusId", "SubdivisionId" },
                values: new object[,]
                {
                    { 1, "Поляков О.А.", "Андреевич", "Олег", "Поляков", "admin1", 1, "0001-0001", 1, 1, 1 },
                    { 2, "Скрипка А.В.", "Викторович", "Андрей", "Скрипка", "admin2", 1, "0001-0002", 1, 1, 1 },
                    { 3, "Фролов И.А.", "Алексеевич", "Иван", "Фролов", "admin3", 1, "0001-0003", 1, 1, 1 },
                    { 4, "Лобанов М.А.", "Александрович", "Михаил", "Лобанов", "admin4", 1, "0001-0004", 1, 1, 1 },
                    { 5, "Кулаков В.А.", "Андреевич", "Виктор", "Кулаков", "admin5", 1, "0001-0005", 1, 1, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_AccessoryTypeId",
                table: "Accessories",
                column: "AccessoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_DetailId",
                table: "Accessories",
                column: "DetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_DeveloperId",
                table: "Accessories",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_Id",
                table: "Accessories",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_OutsideOrganizationId",
                table: "Accessories",
                column: "OutsideOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_TechnologicalProcessItemId",
                table: "Accessories",
                column: "TechnologicalProcessItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryDevelopmentStatuses_AccessoryId",
                table: "AccessoryDevelopmentStatuses",
                column: "AccessoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryDevelopmentStatuses_StatusId",
                table: "AccessoryDevelopmentStatuses",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryEquipments_AccessoryId",
                table: "AccessoryEquipments",
                column: "AccessoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryTypes_Id",
                table: "AccessoryTypes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryTypes_Title",
                table: "AccessoryTypes",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlankTypes_Id",
                table: "BlankTypes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlankTypes_Title",
                table: "BlankTypes",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientProducts_ClientId",
                table: "ClientProducts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Id",
                table: "Clients",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Title",
                table: "Clients",
                column: "Title",
                unique: true);

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
                name: "IX_DetailReplaceabilities_SecondDetailId",
                table: "DetailReplaceabilities",
                column: "SecondDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Details_DetailTypeId",
                table: "Details",
                column: "DetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Details_Id",
                table: "Details",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Details_SerialNumber",
                table: "Details",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Details_UnitId",
                table: "Details",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailsChildren_ChildId",
                table: "DetailsChildren",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailTypes_Id",
                table: "DetailTypes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetailTypes_Title",
                table: "DetailTypes",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentDetailContents_EquipmentDetailId",
                table: "EquipmentDetailContents",
                column: "EquipmentDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentDetailReplacements_EquipmentStatusValueId",
                table: "EquipmentDetailReplacements",
                column: "EquipmentStatusValueId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentDetails_Id",
                table: "EquipmentDetails",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentDetails_SerialNumber",
                table: "EquipmentDetails",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentFailures_Id",
                table: "EquipmentFailures",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentFailures_Title",
                table: "EquipmentFailures",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentParams_Id",
                table: "EquipmentParams",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentParams_Title",
                table: "EquipmentParams",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentParamValues_EquipmentParamId",
                table: "EquipmentParamValues",
                column: "EquipmentParamId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentPlans_TechnologicalProcessItemId",
                table: "EquipmentPlans",
                column: "TechnologicalProcessItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_Id",
                table: "Equipments",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_SerialNumber",
                table: "Equipments",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_SubdivisionId",
                table: "Equipments",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentSchedules_WorkingPartId",
                table: "EquipmentSchedules",
                column: "WorkingPartId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentsOperations_TechnologicalProcessItemId",
                table: "EquipmentsOperations",
                column: "TechnologicalProcessItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentStatuses_Id",
                table: "EquipmentStatuses",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentStatuses_StatusId",
                table: "EquipmentStatuses",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentStatusUsers_EquipmentStatusId",
                table: "EquipmentStatusUsers",
                column: "EquipmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentStatusValues_EquipmentFaulureId",
                table: "EquipmentStatusValues",
                column: "EquipmentFaulureId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentStatusValues_EquipmentId",
                table: "EquipmentStatusValues",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentStatusValues_EquipmentStatusId",
                table: "EquipmentStatusValues",
                column: "EquipmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentStatusValues_Id",
                table: "EquipmentStatusValues",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentStatusValues_UserId",
                table: "EquipmentStatusValues",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentStatusValues_WorkingPartId",
                table: "EquipmentStatusValues",
                column: "WorkingPartId");

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
                name: "IX_Materials_Id",
                table: "Materials",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Title",
                table: "Materials",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoveDetails_Id",
                table: "MoveDetails",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoveDetails_MovedDetailId",
                table: "MoveDetails",
                column: "MovedDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveDetails_MoveTypeId",
                table: "MoveDetails",
                column: "MoveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveDetails_SubdivisionId",
                table: "MoveDetails",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveDetails_TechnologicalProcessItemId",
                table: "MoveDetails",
                column: "TechnologicalProcessItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveTypes_Id",
                table: "MoveTypes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoveTypes_Title",
                table: "MoveTypes",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_Id",
                table: "Operations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutsideOrganizations_Id",
                table: "OutsideOrganizations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutsideOrganizations_Title",
                table: "OutsideOrganizations",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_DetailId",
                table: "Products",
                column: "DetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Id",
                table: "Products",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professions_Id",
                table: "Professions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professions_Title",
                table: "Professions",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClients_UserFormId",
                table: "RoleClients",
                column: "UserFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Id",
                table: "Statuses",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Title",
                table: "Statuses",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subdivisions_FatherId",
                table: "Subdivisions",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_Subdivisions_Id",
                table: "Subdivisions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessData_BlankTypeId",
                table: "TechnologicalProcessData",
                column: "BlankTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessData_Id",
                table: "TechnologicalProcessData",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessData_MaterialId",
                table: "TechnologicalProcessData",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessData_TecnologicalProcessId",
                table: "TechnologicalProcessData",
                column: "TecnologicalProcessId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcesses_DetailId",
                table: "TechnologicalProcesses",
                column: "DetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcesses_DeveloperId",
                table: "TechnologicalProcesses",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcesses_Id",
                table: "TechnologicalProcesses",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessItems_Id",
                table: "TechnologicalProcessItems",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessItems_OperationId",
                table: "TechnologicalProcessItems",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessItems_TechnologicalProcessId",
                table: "TechnologicalProcessItems",
                column: "TechnologicalProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessStatuses_StatusId",
                table: "TechnologicalProcessStatuses",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalProcessStatuses_UserId",
                table: "TechnologicalProcessStatuses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_Id",
                table: "Units",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_Title",
                table: "Units",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserForms_Id",
                table: "UserForms",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfessionId",
                table: "Users",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfessionNumber",
                table: "Users",
                column: "ProfessionNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_StatusId",
                table: "Users",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubdivisionId",
                table: "Users",
                column: "SubdivisionId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessoryDevelopmentStatuses");

            migrationBuilder.DropTable(
                name: "AccessoryEquipments");

            migrationBuilder.DropTable(
                name: "ClientProducts");

            migrationBuilder.DropTable(
                name: "CrossOperationStorages");

            migrationBuilder.DropTable(
                name: "DetailReplaceabilities");

            migrationBuilder.DropTable(
                name: "DetailsChildren");

            migrationBuilder.DropTable(
                name: "EquipmentDetailContents");

            migrationBuilder.DropTable(
                name: "EquipmentDetailReplacements");

            migrationBuilder.DropTable(
                name: "EquipmentParamValues");

            migrationBuilder.DropTable(
                name: "EquipmentPlans");

            migrationBuilder.DropTable(
                name: "EquipmentSchedules");

            migrationBuilder.DropTable(
                name: "EquipmentsOperations");

            migrationBuilder.DropTable(
                name: "EquipmentStatusUsers");

            migrationBuilder.DropTable(
                name: "FinishDetailStorages");

            migrationBuilder.DropTable(
                name: "MoveDetails");

            migrationBuilder.DropTable(
                name: "RoleClients");

            migrationBuilder.DropTable(
                name: "TechnologicalProcessData");

            migrationBuilder.DropTable(
                name: "TechnologicalProcessStatuses");

            migrationBuilder.DropTable(
                name: "Accessories");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "CrossOperationStoragePlaces");

            migrationBuilder.DropTable(
                name: "EquipmentDetails");

            migrationBuilder.DropTable(
                name: "EquipmentStatusValues");

            migrationBuilder.DropTable(
                name: "EquipmentParams");

            migrationBuilder.DropTable(
                name: "FinishedDetailStoragePlaces");

            migrationBuilder.DropTable(
                name: "VirtualStorages");

            migrationBuilder.DropTable(
                name: "MoveTypes");

            migrationBuilder.DropTable(
                name: "UserForms");

            migrationBuilder.DropTable(
                name: "BlankTypes");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "AccessoryTypes");

            migrationBuilder.DropTable(
                name: "OutsideOrganizations");

            migrationBuilder.DropTable(
                name: "EquipmentFailures");

            migrationBuilder.DropTable(
                name: "EquipmentStatuses");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "WorkingParts");

            migrationBuilder.DropTable(
                name: "TechnologicalProcessItems");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "TechnologicalProcesses");

            migrationBuilder.DropTable(
                name: "Details");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "DetailTypes");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Professions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Subdivisions");
        }
    }
}
