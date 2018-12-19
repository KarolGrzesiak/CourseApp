using System.Threading.Tasks;
using CourseApp.API.Helpers;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;

namespace CourseApp.API.Data
{
    public class ExamRepository : RepositoryBase<Exam>, IExamRepository
    {
        private readonly DataContext _context;
        public ExamRepository(DataContext context) : base(context)
        {
            _context = context;

        }

        public Task<Exam> GetExamAsync(int examId)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<Exam>> GetExamsAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}