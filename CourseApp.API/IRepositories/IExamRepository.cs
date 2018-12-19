using System.Threading.Tasks;
using CourseApp.API.Helpers;
using CourseApp.API.Model;

namespace CourseApp.API.IRepositories
{
    public interface IExamRepository : IRepositoryBase<Exam>
    {
        Task<PagedList<Exam>> GetExamsAsync();
        Task<Exam> GetExamAsync(int examId);

    }
}