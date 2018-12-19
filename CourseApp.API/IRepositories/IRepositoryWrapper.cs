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
        IExamRepository ExamRepository { get; }
    }
}