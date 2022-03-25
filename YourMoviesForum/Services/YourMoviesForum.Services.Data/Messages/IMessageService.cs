using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Messages
{
    public interface IMessageService
    {
        Task CreateMessageAsync(string content, string authorId, string receiverId);

        Task<IEnumerable<TModel>> GetAllMessagesAsync<TModel>(string currentUserId);

        Task<string> GetLastActivityAsync(string currentUserId, string userId);

        Task<string> GetLastMessageAsync(string currentUserId, string userId);
    }
}
