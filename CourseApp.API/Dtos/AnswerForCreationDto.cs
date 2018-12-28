using System.ComponentModel.DataAnnotations;

namespace CourseApp.API.Dtos
{
    public class AnswerForCreationDto
    {
        [Required]

        public string Content { get; set; }
        [Required]

        public bool IsCorrect { get; set; }
    }
}