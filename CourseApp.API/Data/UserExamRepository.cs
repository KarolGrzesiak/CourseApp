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
        public async Task<IEnumerable<Exam>> GetExamsForUserAsync(int userId)
        {
            return await _context.UserExams.Include(ur => ur.Exam).Where(ur => ur.UserId == userId).Select(ur => ur.Exam).Include(e=>e.Author).ToListAsync();
        }

    }
}