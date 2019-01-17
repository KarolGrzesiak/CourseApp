using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.API.Data
{
    public class AnswerRepository : RepositoryBase<Answer>, IAnswerRepository
    {
        private readonly DataContext _context;
        public AnswerRepository(DataContext context) : base(context)
        {
            _context = context;

        }
        public async Task<Answer> GetAnswerAsync(int answerId)
        {
            return await _context.Answers.Include(a => a.Question).FirstOrDefaultAsync(a => a.Id == answerId);
        }
        public async Task<IEnumerable<Answer>> GetAnswersAsync(int questionId)
        {
            return await _context.Answers.Where(a => a.QuestionId == questionId).ToListAsync();
        }

    }
}