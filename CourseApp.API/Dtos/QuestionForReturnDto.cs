using System.Collections.Generic;
using CourseApp.API.Model;

namespace CourseApp.API.Dtos
{
    public class QuestionForReturnDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public ICollection<Answer> Answers { get; set; }

    }
}