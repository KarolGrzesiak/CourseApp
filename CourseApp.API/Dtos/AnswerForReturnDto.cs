namespace CourseApp.API.Dtos
{
    public class AnswerForReturnDto
    {

        public int Id { get; set; }
        public string Content { get; set; }

        public bool IsCorrect { get; set; }
    }
}