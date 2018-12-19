using CourseApp.API.IRepositories;
using CourseApp.API.Model;

namespace CourseApp.API.Data
{
    public class UserAnswerRepository : RepositoryBase<UserAnswer>, IUserAnswerRepository
    {
        private readonly DataContext _context;
        public UserAnswerRepository(DataContext context) : base(context)
        {
            _context = context;

        }

    }
}