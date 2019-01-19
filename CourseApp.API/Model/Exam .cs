using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseApp.API.Model
{
    public class Exam
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public string Name { get; set; }
        public DateTime DatePublished { get; set; }
        public string Description { get; set; }
        public ICollection<Question> Questions { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool? Finished { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }


    }
}