namespace CourseApp.API.Model
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        
    }
}