using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApp.API.Model;

namespace CourseApp.API.IRepositories
{
    public interface IUserExamRepository : IRepositoryBase<UserExam>
    {
        Task<IEnumerable<Exam>> GetEnrolledExamsForUserAsync(int userId);
        Task<IEnumerable<Exam>> GetFinishedExamsForUserAsync(int userId);
        Task<UserExam> GetUserWithExamAsync(int userId, int examId);
        Task<IEnumerable<User>> GetUsersFromExamAsync(int examId);
    }
}