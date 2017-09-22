using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Checkmunk.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Checklists");

            migrationBuilder.EnsureSchema(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "ChecklistUser",
                schema: "Checklists",
                columns: table => new
                {
                    PersistenceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistUser", x => x.PersistenceId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Users",
                columns: table => new
                {
                    PersistenceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.PersistenceId);
                });

            migrationBuilder.CreateTable(
                name: "Checklist",
                schema: "Checklists",
                columns: table => new
                {
                    PersistenceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedByPersistenceId = table.Column<int>(type: "INTEGER", nullable: true),
                    Id = table.Column<Guid>(type: "BLOB", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checklist", x => x.PersistenceId);
                    table.ForeignKey(
                        name: "FK_Checklist_ChecklistUser_CreatedByPersistenceId",
                        column: x => x.CreatedByPersistenceId,
                        principalSchema: "Checklists",
                        principalTable: "ChecklistUser",
                        principalColumn: "PersistenceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistItem",
                schema: "Checklists",
                columns: table => new
                {
                    PersistenceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChecklistPersistenceId = table.Column<int>(type: "INTEGER", nullable: true),
                    Id = table.Column<Guid>(type: "BLOB", nullable: false),
                    IsChecked = table.Column<bool>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistItem", x => x.PersistenceId);
                    table.ForeignKey(
                        name: "FK_ChecklistItem_Checklist_ChecklistPersistenceId",
                        column: x => x.ChecklistPersistenceId,
                        principalSchema: "Checklists",
                        principalTable: "Checklist",
                        principalColumn: "PersistenceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Checklist_CreatedByPersistenceId",
                schema: "Checklists",
                table: "Checklist",
                column: "CreatedByPersistenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItem_ChecklistPersistenceId",
                schema: "Checklists",
                table: "ChecklistItem",
                column: "ChecklistPersistenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistItem",
                schema: "Checklists");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Users");

            migrationBuilder.DropTable(
                name: "Checklist",
                schema: "Checklists");

            migrationBuilder.DropTable(
                name: "ChecklistUser",
                schema: "Checklists");
        }
    }
}
