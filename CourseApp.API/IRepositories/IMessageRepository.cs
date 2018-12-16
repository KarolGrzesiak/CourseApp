using System.Collections.Generic;
using System.Threading.Tasks;
using CourseApp.API.Helpers;
using CourseApp.API.Model;

namespace CourseApp.API.IRepositories
{
    public interface IMessageRepository : IRepositoryBase<Message>
    {
        Task<Message> GetMessageAsync(int id);
        Task<PagedList<Message>> GetMessagesForUserAsync(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThreadAsync(int userId, int recipientId);
    }
}