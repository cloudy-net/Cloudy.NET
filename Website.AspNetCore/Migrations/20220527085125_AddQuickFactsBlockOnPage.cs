using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.AspNetCore.Migrations
{
    public partial class AddQuickFactsBlockOnPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Facts",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facts",
                table: "Pages");
        }
    }
}
