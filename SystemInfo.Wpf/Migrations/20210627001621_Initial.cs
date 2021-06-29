using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SystemInfo.Wpf.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enterprises",
                columns: table => new
                {
                    RNC = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enterprises", x => x.RNC);
                });

            migrationBuilder.CreateTable(
                name: "SystemSpecs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MachineName = table.Column<string>(type: "TEXT", nullable: true),
                    OperatingSystemVersion = table.Column<string>(type: "TEXT", nullable: true),
                    IsOperatingSystem64bits = table.Column<bool>(type: "INTEGER", nullable: false),
                    TotalMemoryInGigaBytes = table.Column<int>(type: "INTEGER", nullable: false),
                    ProcessorName = table.Column<string>(type: "TEXT", nullable: true),
                    ProcessorCount = table.Column<int>(type: "INTEGER", nullable: false),
                    EnterpriseRNC = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSpecs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemSpecs_Enterprises_EnterpriseRNC",
                        column: x => x.EnterpriseRNC,
                        principalTable: "Enterprises",
                        principalColumn: "RNC",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WindowsAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    SystemSpecsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WindowsAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WindowsAccount_SystemSpecs_SystemSpecsId",
                        column: x => x.SystemSpecsId,
                        principalTable: "SystemSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemSpecs_EnterpriseRNC",
                table: "SystemSpecs",
                column: "EnterpriseRNC");

            migrationBuilder.CreateIndex(
                name: "IX_WindowsAccount_SystemSpecsId",
                table: "WindowsAccount",
                column: "SystemSpecsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WindowsAccount");

            migrationBuilder.DropTable(
                name: "SystemSpecs");

            migrationBuilder.DropTable(
                name: "Enterprises");
        }
    }
}
