using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApp.API.Model;

namespace CourseApp.API.IRepositories
{
    public interface IQuestionRepository : IRepositoryBase<Question>
    {
        Task<Question> GetQuestionAsync(int questionId);
        Task<IEnumerable<Question>> GetQuestionsAsync(int examId);
    }
}