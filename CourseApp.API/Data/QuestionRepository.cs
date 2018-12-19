using CourseApp.API.IRepositories;
using CourseApp.API.Model;

namespace CourseApp.API.Data
{
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        private readonly DataContext _context;
        public QuestionRepository(DataContext context) : base(context)
        {
            _context = context;

        }

    }
}