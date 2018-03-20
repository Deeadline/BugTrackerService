using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTrackerService.Migrations
{
    public partial class DBTickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Products_ProductId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ProductId",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_TicketId",
                table: "Products",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Tickets_TicketId",
                table: "Products",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Tickets_TicketId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_TicketId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProductId",
                table: "Tickets",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Products_ProductId",
                table: "Tickets",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
