using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StaffFlow_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DayTypes",
                columns: table => new
                {
                    DayTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DayTypeName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayTypes", x => x.DayTypeId);
                });

            migrationBuilder.CreateTable(
                name: "SensorData",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    temperature = table.Column<decimal>(type: "numeric", nullable: false),
                    humidity = table.Column<decimal>(type: "numeric", nullable: false),
                    IsTemperatureAdjustmentEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsHumidityAdjustmentEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorData", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequests",
                columns: table => new
                {
                    ChangeRequestId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    DayTypeId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequests", x => x.ChangeRequestId);
                    table.ForeignKey(
                        name: "FK_ChangeRequests_DayTypes_DayTypeId",
                        column: x => x.DayTypeId,
                        principalTable: "DayTypes",
                        principalColumn: "DayTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingDays",
                columns: table => new
                {
                    WorkingDayId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DayTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDays", x => x.WorkingDayId);
                    table.ForeignKey(
                        name: "FK_WorkingDays_DayTypes_DayTypeId",
                        column: x => x.DayTypeId,
                        principalTable: "DayTypes",
                        principalColumn: "DayTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserChangeRequests",
                columns: table => new
                {
                    UserChangeRequestId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ChangeRequestId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChangeRequests", x => x.UserChangeRequestId);
                    table.ForeignKey(
                        name: "FK_UserChangeRequests_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChangeRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWorkingDays",
                columns: table => new
                {
                    UserWorkingDayId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    WorkingDayId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkingDays", x => x.UserWorkingDayId);
                    table.ForeignKey(
                        name: "FK_UserWorkingDays_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWorkingDays_WorkingDays_WorkingDayId",
                        column: x => x.WorkingDayId,
                        principalTable: "WorkingDays",
                        principalColumn: "WorkingDayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequests_DayTypeId",
                table: "ChangeRequests",
                column: "DayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChangeRequests_ChangeRequestId",
                table: "UserChangeRequests",
                column: "ChangeRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChangeRequests_UserId",
                table: "UserChangeRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkingDays_UserId",
                table: "UserWorkingDays",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkingDays_WorkingDayId",
                table: "UserWorkingDays",
                column: "WorkingDayId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingDays_DayTypeId",
                table: "WorkingDays",
                column: "DayTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorData");

            migrationBuilder.DropTable(
                name: "UserChangeRequests");

            migrationBuilder.DropTable(
                name: "UserWorkingDays");

            migrationBuilder.DropTable(
                name: "ChangeRequests");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WorkingDays");

            migrationBuilder.DropTable(
                name: "DayTypes");
        }
    }
}
