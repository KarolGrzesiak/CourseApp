using System.Threading.Tasks;
using CourseApp.API.Model;

namespace CourseApp.API.IRepositories
{
    public interface IPhotoRepository : IRepositoryBase<Photo>
    {
        Task<Photo> GetPhotoAsync(int id);
        Task<Photo> GetMainPhotoForUserAsync(int userId);
    }
}