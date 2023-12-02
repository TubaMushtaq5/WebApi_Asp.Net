using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DL.Migrations
{
    public partial class marks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Marks",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "marks",
                table: "StudentSubjects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "marks",
                table: "StudentSubjects");

            migrationBuilder.AddColumn<int>(
                name: "Marks",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
