using Microsoft.EntityFrameworkCore.Migrations;

namespace Doctors_Web_Forum_FE.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeToken",
                table: "Account",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeToken",
                table: "Account");
        }
    }
}
