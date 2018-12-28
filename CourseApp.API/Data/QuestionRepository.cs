using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.API.Data
{
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        private readonly DataContext _context;
        public QuestionRepository(DataContext context) : base(context)
        {
            _context = context;

        }

        public async Task<Question> GetQuestionAsync(int examId, int questionId)
        {
            return await _context.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == questionId && q.ExamId == examId);
        }

        public async Task<IEnumerable<Question>> GetQuestionsAsync(int examId)
        {
            return await _context.Questions.Include(q => q.Answers).Where(q => q.ExamId == examId).ToListAsync();
        }
    }
}