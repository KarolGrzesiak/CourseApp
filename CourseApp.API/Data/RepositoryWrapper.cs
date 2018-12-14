using CourseApp.API.IRepositories;

namespace CourseApp.API.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IUserRepository _userRepository;
        private DataContext _context;

        public RepositoryWrapper(DataContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }
    }
}