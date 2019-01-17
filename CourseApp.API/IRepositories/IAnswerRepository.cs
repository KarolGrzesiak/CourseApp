using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApp.API.Model;

namespace CourseApp.API.IRepositories
{
    public interface IAnswerRepository :IRepositoryBase<Answer>
    {
        Task<Answer> GetAnswerAsync(int answerId);
        Task<IEnumerable<Answer>> GetAnswersAsync(int questionId);
    }
}