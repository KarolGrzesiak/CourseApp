using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.API.Data
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context) : base(context)
        {
            _context = context;

        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await FindAll().Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await FindAll().Include(u => u.Photos).ToListAsync();
        }


    }
}