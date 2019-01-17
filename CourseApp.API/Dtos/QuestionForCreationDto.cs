using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CourseApp.API.Model;

namespace CourseApp.API.Dtos
{
    public class QuestionForCreationDto
    {
        [Required]
        public string Content { get; set; }
        public ICollection<AnswerForCreationDto> Answers { get; set; }
        public string Type { get; set; }
    }
}