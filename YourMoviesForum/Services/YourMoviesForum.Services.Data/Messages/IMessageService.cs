using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Messages
{
    public interface IMessageService
    {
        Task CreateMessageAsync(string content, string authorId, string receiverId);
    }
}
