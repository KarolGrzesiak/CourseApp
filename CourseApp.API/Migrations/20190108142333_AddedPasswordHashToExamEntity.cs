using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseApp.API.Migrations
{
    public partial class AddedPasswordHashToExamEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Exams",
                nullable: false,
                defaultValue: new byte[] {  });

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Exams",
                nullable: false,
                defaultValue: new byte[] {  });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Exams");
        }
    }
}
