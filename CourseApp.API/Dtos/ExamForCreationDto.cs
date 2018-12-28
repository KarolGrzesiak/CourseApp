using System;
using System.ComponentModel.DataAnnotations;

namespace CourseApp.API.Dtos
{
    public class ExamForCreationDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTime DatePublished { get; set; }

        public ExamForCreationDto()
        {
            DatePublished = DateTime.Now;
        }
    }
}