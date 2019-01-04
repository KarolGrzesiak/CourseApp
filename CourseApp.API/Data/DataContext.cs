using CourseApp.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.API.Data
{
    public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>
    , IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserExam> UserExams { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<UserRole>()
                    .HasKey(ur => new { ur.UserId, ur.RoleId });
            builder.Entity<UserRole>()
                    .HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

            builder.Entity<UserRole>()
                     .HasOne(ur => ur.Role)
                     .WithMany(r => r.UserRoles)
                     .HasForeignKey(ur => ur.RoleId)
                     .IsRequired();




            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany(u => u.MessagesRecivied)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<UserExam>()
                    .HasKey(ue => new { ue.UserId, ue.ExamId });

            builder.Entity<UserAnswer>()
                    .HasKey(ua => new { ua.UserId, ua.AnswerId });

        }
    }
}