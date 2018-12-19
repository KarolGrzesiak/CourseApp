using CourseApp.API.IRepositories;
using CourseApp.API.Model;

namespace CourseApp.API.Data
{
    public class AnswerRepository : RepositoryBase<Answer>, IAnswerRepository
    {
        private readonly DataContext _context;
        public AnswerRepository(DataContext context) : base(context)
        {
            _context = context;

        }

    }
}