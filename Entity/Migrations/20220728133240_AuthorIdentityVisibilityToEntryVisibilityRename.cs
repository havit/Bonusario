using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Havit.Bonusario.Entity.Migrations
{
    public partial class AuthorIdentityVisibilityToEntryVisibilityRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorIdentityVisibility",
                table: "Entry",
                newName: "Visibility");

            migrationBuilder.RenameColumn(
                name: "DefaultIdentityVisibility",
                table: "Employee",
                newName: "DefaultEntryVisibility");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Visibility",
                table: "Entry",
                newName: "AuthorIdentityVisibility");

            migrationBuilder.RenameColumn(
                name: "DefaultEntryVisibility",
                table: "Employee",
                newName: "DefaultIdentityVisibility");
        }
    }
}
