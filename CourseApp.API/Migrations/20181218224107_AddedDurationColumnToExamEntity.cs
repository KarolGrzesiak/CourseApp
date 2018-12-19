using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseApp.API.Migrations
{
    public partial class AddedDurationColumnToExamEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Exams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Exams");
        }
    }
}
