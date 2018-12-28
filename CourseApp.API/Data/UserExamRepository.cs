using CourseApp.API.IRepositories;
using CourseApp.API.Model;

namespace CourseApp.API.Data
{
    public class UserExamRepository : RepositoryBase<UserExam>, IUserExamRepository
    {
        private readonly DataContext _context;
        public UserExamRepository(DataContext context) : base(context)
        {
            _context = context;

        }
    }
}