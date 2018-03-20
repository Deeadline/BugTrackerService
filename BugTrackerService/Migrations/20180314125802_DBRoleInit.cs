using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTrackerService.Migrations
{
    public partial class DBRoleInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Assigned",
                table: "Tickets",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Assigned",
                table: "Tickets");
        }
    }
}
