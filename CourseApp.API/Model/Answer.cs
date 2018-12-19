namespace CourseApp.API.Model
{
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool isCorrect { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        
    }
}