using Microsoft.EntityFrameworkCore.Migrations;

namespace Website.AspNetCore.Migrations
{
    public partial class AddPageAddMainBody : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainBody",
                table: "Pages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainBody",
                table: "Pages");
        }
    }
}
