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
        public static int MinAge { get; } = 1;
        public static int MaxAge { get; } = 99;
        public static int MaxPageSize { get; } = 50;
        public static int PageSize { get; } = 6;

    }
}