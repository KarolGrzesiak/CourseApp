namespace CourseApp.API.IRepositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; }
        IPhotoRepository PhotoRepository { get; }
        IMessageRepository MessageRepository { get; }
    }
}