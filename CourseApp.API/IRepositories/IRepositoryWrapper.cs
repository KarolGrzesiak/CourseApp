using System.Threading.Tasks;

namespace CourseApp.API.IRepositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; }
        IPhotoRepository PhotoRepository { get; }
        IMessageRepository MessageRepository { get; }
        IAnswerRepository AnswerRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IUserAnswerRepository UserAnswerRepository { get; }
        IUserExamRepository UserExamRepository { get; }
        IExamRepository ExamRepository { get; }
        Task<bool> SaveAllAsync();
    }
}