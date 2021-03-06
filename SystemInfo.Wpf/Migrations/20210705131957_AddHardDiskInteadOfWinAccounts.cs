using Microsoft.EntityFrameworkCore.Migrations;

namespace SystemInfo.Wpf.Migrations
{
    public partial class AddHardDiskInteadOfWinAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WindowsAccount");

            migrationBuilder.CreateTable(
                name: "HardDisk",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Label = table.Column<string>(type: "TEXT", nullable: true),
                    SizeInGigabytes = table.Column<int>(type: "INTEGER", nullable: false),
                    FreeSpaceInGigabytes = table.Column<int>(type: "INTEGER", nullable: false),
                    SystemSpecsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardDisk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HardDisk_SystemSpecs_SystemSpecsId",
                        column: x => x.SystemSpecsId,
                        principalTable: "SystemSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HardDisk_SystemSpecsId",
                table: "HardDisk",
                column: "SystemSpecsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HardDisk");

            migrationBuilder.CreateTable(
                name: "WindowsAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SystemSpecsId = table.Column<int>(type: "INTEGER", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "IX_WindowsAccount_SystemSpecsId",
                table: "WindowsAccount",
                column: "SystemSpecsId");
        }
    }
}
