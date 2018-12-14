namespace CourseApp.API.IRepositories
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; }
    }
}