using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Havit.Bonusario.Entity.Migrations
{
    public partial class AddDefaultAuthorIdentityVisibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultIdentityVisibility",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultIdentityVisibility",
                table: "Employee");
        }
    }
}
