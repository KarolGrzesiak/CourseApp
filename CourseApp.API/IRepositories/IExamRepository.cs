using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApp.API.Helpers;
using CourseApp.API.Model;

namespace CourseApp.API.IRepositories
{
    public interface IExamRepository : IRepositoryBase<Exam>
    {
        Task<PagedList<Exam>> GetNotEnrolledExamsForUserAsync(int? pageNumber, int? pageSize, int userId);
        Task<Exam> GetExamAsync(int examId);
        Task<IEnumerable<Exam>> GetCreatedExamsForUserAsync(int userId);
    }
}