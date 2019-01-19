using System;

namespace CourseApp.API.Dtos
{
    public class ExamForListDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string AuthorKnownAs { get; set; }
        public DateTime DatePublished { get; set; }
        public TimeSpan? Duration { get; set; }
        
    }
}