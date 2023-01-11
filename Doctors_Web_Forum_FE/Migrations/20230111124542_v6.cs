using Microsoft.EntityFrameworkCore.Migrations;

namespace Doctors_Web_Forum_FE.Migrations
{
    public partial class v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "Work_Place",
                table: "Account",
                newName: "Location");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Account",
                newName: "Work_Place");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Account",
                maxLength: 200,
                nullable: true);
        }
    }
}
