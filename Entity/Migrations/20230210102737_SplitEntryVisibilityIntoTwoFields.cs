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

			migrationBuilder.Sql("UPDATE Entry SET [Public] = 0, Signed = 0 WHERE Visibility = 0");
			migrationBuilder.Sql("UPDATE Entry SET [Public] = 0, Signed = 1 WHERE Visibility = 1");
			migrationBuilder.Sql("UPDATE Entry SET [Public] = 1, Signed = 1 WHERE Visibility = 2");
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

			migrationBuilder.Sql("UPDATE Entry SET Visibility = 0 WHERE [Public] = 0, Signed = 0");
			migrationBuilder.Sql("UPDATE Entry SET Visibility = 0 WHERE [Public] = 1, Signed = 0");
			migrationBuilder.Sql("UPDATE Entry SET Visibility = 1 WHERE [Public] = 0, Signed = 1");
			migrationBuilder.Sql("UPDATE Entry SET Visibility = 2 WHERE [Public] = 1, Signed = 1");
		}
    }
}
