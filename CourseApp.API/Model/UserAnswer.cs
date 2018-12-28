namespace CourseApp.API.Model
{
    public class UserAnswer
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int AnswerId { get; set; }
        public Answer Answer { get; set; }
        public string Content { get; set; }

    }
}