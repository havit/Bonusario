using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Havit.Bonusario.Entity.Migrations
{
    public partial class AuthorIdentityVisibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorIdentityVisibility",
                table: "Entry",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorIdentityVisibility",
                table: "Entry");
        }
    }
}
