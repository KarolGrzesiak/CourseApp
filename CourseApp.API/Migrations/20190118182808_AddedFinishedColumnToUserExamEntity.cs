using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseApp.API.Migrations
{
    public partial class AddedFinishedColumnToUserExamEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "UserExams",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Exams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Exams");
        }
    }
}
