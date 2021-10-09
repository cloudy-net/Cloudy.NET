using Microsoft.EntityFrameworkCore.Migrations;

namespace Website.AspNetCore.Migrations
{
    public partial class PageAddRouteSegment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlSegment",
                table: "Pages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlSegment",
                table: "Pages");
        }
    }
}
