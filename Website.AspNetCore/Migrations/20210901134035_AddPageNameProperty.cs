using Microsoft.EntityFrameworkCore.Migrations;

namespace Website.AspNetCore.Migrations
{
    public partial class AddPageNameProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Pages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Pages");
        }
    }
}
