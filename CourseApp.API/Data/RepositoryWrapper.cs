using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.AspNetCore.Identity;

namespace CourseApp.API.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IUserRepository _userRepository;
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public RepositoryWrapper(DataContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context,_userManager);
                }
                return _userRepository;
            }
        }
    }
}