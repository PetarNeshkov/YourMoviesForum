using System;
using System.Threading.Tasks;
using YourMoviesForum.Data.Models;

namespace YourMoviesForum.Services.Data.Messages
{
    public class MessageService : IMessageService
    {
        private readonly YourMoviesDbContext data;

        public MessageService(YourMoviesDbContext data)
        {
            this.data = data;
        }

        public async Task CreateAsync(string content, string authorId, string receiverId)
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
    }
}
