using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Events.Data.Migrations.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    LockedDate = table.Column<DateTimeOffset>(nullable: true),
                    Capacity = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCategoryTickets",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Penalty = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCategoryTickets", x => new { x.UserId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_UserCategoryTickets_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCategoryTickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEventSignUps",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    EventId = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEventSignUps", x => new { x.EventId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserEventSignUps_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEventSignUps_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Id value removed so the rows can use the auto increment.
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "Badminton" },
                    { "Football" },
                    { "Social" },
                    { "Video games" },
                    { "Board games" }
                });

            // Id value removed so the rows can use the auto increment.
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Capacity", "CategoryId", "Date", "Location", "LockedDate" },
                values: new object[,]
                {
                    { 8, 1, new DateTimeOffset(new DateTime(2020, DateTimeOffset.Now.Month, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Stratford Leisure Centre", null },
                    { 8, 1, new DateTimeOffset(new DateTime(2020, DateTimeOffset.Now.Month, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Stratford Leisure Centre", null },
                    { 8, 1, new DateTimeOffset(new DateTime(2020, DateTimeOffset.Now.Month, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Stratford Leisure Centre", null },
                    { 8, 1, new DateTimeOffset(new DateTime(2020, DateTimeOffset.Now.Month, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Stratford Leisure Centre", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CategoryId",
                table: "Events",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCategoryTickets_CategoryId",
                table: "UserCategoryTickets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEventSignUps_UserId",
                table: "UserEventSignUps",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCategoryTickets");

            migrationBuilder.DropTable(
                name: "UserEventSignUps");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
