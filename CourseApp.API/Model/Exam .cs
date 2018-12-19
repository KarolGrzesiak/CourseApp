using System;
using System.Collections.Generic;

namespace CourseApp.API.Model
{
    public class Exam
    {
        public int Id { get; set; }
        public DateTime DatePublished { get; set; }
        public string Description { get; set; }
        public ICollection<Question> Questions { get; set; }
        public TimeSpan? Duration { get; set; }

        public Exam()
        {
            DatePublished = DateTime.Now;
        }
    }
}