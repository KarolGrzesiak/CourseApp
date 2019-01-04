using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApp.API.Model;

namespace CourseApp.API.IRepositories
{
    public interface IUserExamRepository : IRepositoryBase<UserExam>
    {
        Task<IEnumerable<Exam>> GetExamsForUserAsync(int userId);
    }
}