using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Havit.Bonusario.Entity.Migrations
{
    public partial class InitialAdjusments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Period",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Entry",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Entry",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PeriodId",
                table: "Entry",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Submitted",
                table: "Entry",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Employee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entry_PeriodId",
                table: "Entry",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Email",
                table: "Employee",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Entry_Period_PeriodId",
                table: "Entry",
                column: "PeriodId",
                principalTable: "Period",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entry_Period_PeriodId",
                table: "Entry");

            migrationBuilder.DropIndex(
                name: "IX_Entry_PeriodId",
                table: "Entry");

            migrationBuilder.DropIndex(
                name: "IX_Employee_Email",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Period");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "PeriodId",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "Submitted",
                table: "Entry");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Employee");
        }
    }
}
