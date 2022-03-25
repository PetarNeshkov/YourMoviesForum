using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using YourMoviesForum.Data.Models;

namespace YourMoviesForum.Services.Data.Messages
{
    public class MessageService : IMessageService
    {
        private readonly YourMoviesDbContext data;
        private readonly IMapper mapper;

        public MessageService(YourMoviesDbContext data,IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task CreateMessageAsync(string content, string authorId, string receiverId)
        {
            var message = new Message
            {
                Content = content,
                AuthorId = authorId,
                ReceiverId = receiverId
            };

            await data.Messages.AddAsync(message);
            await data.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllMessagesAsync<TModel>(string currentUserId)
        {
            var sentMessages = data.Messages
               .Where(m => m.AuthorId == currentUserId || m.ReceiverId == currentUserId)
               .OrderByDescending(m => m.CreatedOn)
               .Select(m => m.Author);

            var receivedMessages = data.Messages
                .Where(m => m.AuthorId == currentUserId || m.ReceiverId == currentUserId)
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => m.Receiver);

                var concatenatedMessages = await sentMessages
                .Concat(receivedMessages)
                .Where(u => u.Id != currentUserId)
                .Distinct()
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return concatenatedMessages;
        }

        public async Task<string> GetLastActivityAsync(string currentUserId, string userId)
           => await data.Messages
                 .Where(m => (m.ReceiverId == currentUserId && m.AuthorId == userId) ||
                             (m.ReceiverId == userId && m.AuthorId == currentUserId))
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => m.CreatedOn)
                .FirstOrDefaultAsync();

        public async Task<string> GetLastMessageAsync(string currentUserId, string userId)
            => await data.Messages
                .Where(m =>  (m.ReceiverId == currentUserId && m.AuthorId == userId) ||
                             (m.ReceiverId == userId && m.AuthorId == currentUserId))
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => m.Content)
                .FirstOrDefaultAsync();
    }
}
