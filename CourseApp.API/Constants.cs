using System;

namespace CourseApp.API
{
    public static class Constants
    {
        public static string TeacherRole { get; } = "Teacher";
        public static string StudentRole { get; } = "Student";
        public static string AdminRole { get; } = "Admin";
        public static string Password { get; } = "password";
        public static int DaysToAddToTokenExpirationDate { get; } = 1;
    }
}