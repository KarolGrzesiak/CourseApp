using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.AspNetCore.Identity;

namespace CourseApp.API.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IUserRepository _userRepository;
        private IPhotoRepository _photoRepository;

        private readonly DataContext _context;

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
        public IPhotoRepository PhotoRepository
        {
            get
            {
                if (_photoRepository == null)
                {
                    _photoRepository = new PhotoRepository(_context);
                }
                return _photoRepository;
            }
        }
    }
}