using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BagoLootAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Child",
                columns: table => new
                {
                    ChildId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Delivered = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Child", x => x.ChildId);
                });

            migrationBuilder.CreateTable(
                name: "Reindeer",
                columns: table => new
                {
                    ReindeerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reindeer", x => x.ReindeerId);
                });

            migrationBuilder.CreateTable(
                name: "Toy",
                columns: table => new
                {
                    ToyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChildId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toy", x => x.ToyId);
                    table.ForeignKey(
                        name: "FK_Toy_Child_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Child",
                        principalColumn: "ChildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteReindeer",
                columns: table => new
                {
                    FavoriteReindeerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChildId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReindeerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteReindeer", x => x.FavoriteReindeerId);
                    table.ForeignKey(
                        name: "FK_FavoriteReindeer_Child_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Child",
                        principalColumn: "ChildId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteReindeer_Reindeer_ReindeerId",
                        column: x => x.ReindeerId,
                        principalTable: "Reindeer",
                        principalColumn: "ReindeerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteReindeer_ChildId",
                table: "FavoriteReindeer",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteReindeer_ReindeerId",
                table: "FavoriteReindeer",
                column: "ReindeerId");

            migrationBuilder.CreateIndex(
                name: "IX_Toy_ChildId",
                table: "Toy",
                column: "ChildId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteReindeer");

            migrationBuilder.DropTable(
                name: "Toy");

            migrationBuilder.DropTable(
                name: "Reindeer");

            migrationBuilder.DropTable(
                name: "Child");
        }
    }
}
