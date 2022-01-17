using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Havit.Bonusario.Entity.Migrations
{
    public partial class PeriodSetAdition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PeriodSetId",
                table: "Period",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PeriodSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodSet", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Period_PeriodSetId",
                table: "Period",
                column: "PeriodSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Period_PeriodSet_PeriodSetId",
                table: "Period",
                column: "PeriodSetId",
                principalTable: "PeriodSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Period_PeriodSet_PeriodSetId",
                table: "Period");

            migrationBuilder.DropTable(
                name: "PeriodSet");

            migrationBuilder.DropIndex(
                name: "IX_Period_PeriodSetId",
                table: "Period");

            migrationBuilder.DropColumn(
                name: "PeriodSetId",
                table: "Period");
        }
    }
}
