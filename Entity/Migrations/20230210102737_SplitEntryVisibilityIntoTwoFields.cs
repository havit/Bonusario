using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Havit.Bonusario.Entity.Migrations
{
    public partial class SplitEntryVisibilityIntoTwoFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultEntryVisibility",
                table: "Employee");

            migrationBuilder.AddColumn<bool>(
                name: "Public",
                table: "Entry",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Signed",
                table: "Entry",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Public",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "Signed",
                table: "Entry");

            migrationBuilder.AddColumn<int>(
                name: "DefaultEntryVisibility",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
