using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.API.Data
{
    public class UserAnswerRepository : RepositoryBase<UserAnswer>, IUserAnswerRepository
    {
        private readonly DataContext _context;
        public UserAnswerRepository(DataContext context) : base(context)
        {
            _context = context;

        }
        public async Task<IEnumerable<UserAnswer>> GetUserAnswersAsync(int examId, int userId)
        {
            return await _context.UserAnswers.Include(ur => ur.Question).Where(ur => ur.UserId == userId && ur.Question.ExamId == examId).ToListAsync();
        }

    }
}