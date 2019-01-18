using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApp.API.Model;

namespace CourseApp.API.IRepositories
{
    public interface IUserAnswerRepository : IRepositoryBase<UserAnswer>
    {
        Task<IEnumerable<UserAnswer>> GetUserAnswersAsync(int examId, int userId);
    }
}