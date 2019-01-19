using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.API.Data
{
    public class UserExamRepository : RepositoryBase<UserExam>, IUserExamRepository
    {
        private readonly DataContext _context;
        public UserExamRepository(DataContext context) : base(context)
        {
            _context = context;

        }
        public async Task<IEnumerable<Exam>> GetEnrolledExamsForUserAsync(int userId)
        {
            return await _context.UserExams.Include(ur => ur.Exam).Where(ur => ur.UserId == userId && ur.Finished == false).Select(ur => ur.Exam).Include(e => e.Author).ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetFinishedExamsForUserAsync(int userId)
        {
            return await _context.UserExams.Include(ur => ur.Exam).Where(ur => ur.UserId == userId && ur.Finished == true).Select(ur => ur.Exam).Include(e => e.Author).ToListAsync();
        }
        public async Task<UserExam> GetUserWithExamAsync(int userId, int examId)
        {
            return await _context.UserExams.Include(ur => ur.Exam).FirstOrDefaultAsync(ur => ur.UserId == userId && ur.ExamId == examId);
        }
        public async Task<IEnumerable<User>> GetUsersFromExamAsync(int examId)
        {
            return await _context.UserExams.Include(ur => ur.User).Where(ur => ur.ExamId == examId).Select(ur => ur.User).ToListAsync();
        }

    }
}