using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApp.API.Model;

namespace CourseApp.API.IRepositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserAsync(int userId);
        Task<IEnumerable<User>> GetUsersAsync();

    }
}