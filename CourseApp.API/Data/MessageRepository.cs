using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApp.API.Helpers;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.API.Data
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        private readonly DataContext _context;
        public MessageRepository(DataContext context) : base(context)
        {
            _context = context;

        }
        public async Task<Message> GetMessageAsync(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUserAsync(MessageParams messageParams)
        {
            var messages = _context.Messages.Include(m => m.Sender).ThenInclude(u => u.Photos)
                                            .Include(m => m.Recipient).ThenInclude(u => u.Photos)
                                            .AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(m => m.RecipientId == messageParams.UserId && m.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(m => m.SenderId == messageParams.UserId && m.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(m => m.RecipientId == messageParams.UserId && m.RecipientDeleted == false && m.IsRead == false);
                    break;
            }
            messages = messages.OrderByDescending(m => m.MessageSent);

            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThreadAsync(int userId, int recipientId)
        {
            var messages = await _context.Messages.Include(m => m.Sender).ThenInclude(u => u.Photos)
                                            .Include(m => m.Recipient).ThenInclude(u => u.Photos)
                                            .Where(m => m.RecipientId == userId && m.RecipientDeleted == false && m.SenderId == recipientId || m.RecipientId == recipientId && m.SenderDeleted == false && m.SenderId == userId)
                                            .OrderByDescending(m => m.MessageSent)
                                            .ToListAsync();
            return messages;
        }
    }
}