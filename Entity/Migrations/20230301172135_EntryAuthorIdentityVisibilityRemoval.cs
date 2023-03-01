using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Havit.Bonusario.Entity.Migrations
{
    public partial class EntryAuthorIdentityVisibilityRemoval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "Entry");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "Entry",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
