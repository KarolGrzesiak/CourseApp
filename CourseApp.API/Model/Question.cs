using System.Collections.Generic;

namespace CourseApp.API.Model
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public string Type { get; set; }
    }
}