using System.Linq;
using System.Threading.Tasks;
using CourseApp.API.Helpers;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.API.Data
{
    public class ExamRepository : RepositoryBase<Exam>, IExamRepository
    {
        private readonly DataContext _context;
        public ExamRepository(DataContext context) : base(context)
        {
            _context = context;

        }

        public async Task<Exam> GetExamAsync(int examId)
        {

            return await _context.Exams.Include(e => e.Questions).FirstOrDefaultAsync(e => e.Id == examId);
        }


        public async Task<PagedList<Exam>> GetExamsAsync(int? pageNumber, int? pageSize, int userId)
        {
            var enrolledExams = await _context.UserExams.Where(ue => ue.UserId == userId).Select(ue => ue.Exam).ToListAsync();
            var exams = _context.Exams.Include(e => e.Questions).Include(e => e.Author).OrderByDescending(e => e.DatePublished)
                                        .Where(e => !enrolledExams.Contains(e)).AsQueryable();
            return await PagedList<Exam>.CreateAsync(exams, pageNumber ?? 1, pageSize ?? Constants.PageSize);
        }

    }
}