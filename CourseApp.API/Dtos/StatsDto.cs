namespace CourseApp.API.Dtos
{
    public class StatsDto
    {
        public string UserName { get; set; }
        public int NumberOfWrongAnswers { get; set; }
        public int NumberOfCorrectAnswers { get; set; }
    }
}