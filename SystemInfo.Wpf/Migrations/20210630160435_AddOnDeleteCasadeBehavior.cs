using Microsoft.EntityFrameworkCore.Migrations;

namespace SystemInfo.Wpf.Migrations
{
    public partial class AddOnDeleteCasadeBehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemSpecs_Enterprises_EnterpriseRNC",
                table: "SystemSpecs");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSpecs_Enterprises_EnterpriseRNC",
                table: "SystemSpecs",
                column: "EnterpriseRNC",
                principalTable: "Enterprises",
                principalColumn: "RNC",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemSpecs_Enterprises_EnterpriseRNC",
                table: "SystemSpecs");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSpecs_Enterprises_EnterpriseRNC",
                table: "SystemSpecs",
                column: "EnterpriseRNC",
                principalTable: "Enterprises",
                principalColumn: "RNC",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
