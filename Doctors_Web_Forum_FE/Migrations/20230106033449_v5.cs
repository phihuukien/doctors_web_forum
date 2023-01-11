using Microsoft.EntityFrameworkCore.Migrations;

namespace Doctors_Web_Forum_FE.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CodeToken",
                table: "Account",
                newName: "Token");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Account",
                newName: "CodeToken");
        }
    }
}
