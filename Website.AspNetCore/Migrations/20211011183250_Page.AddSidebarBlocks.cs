using Microsoft.EntityFrameworkCore.Migrations;

namespace Website.AspNetCore.Migrations
{
    public partial class PageAddSidebarBlocks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SidebarBlocks",
                table: "Pages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SidebarBlocks",
                table: "Pages");
        }
    }
}
