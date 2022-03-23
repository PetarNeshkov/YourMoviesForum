using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Messages
{
    public interface IMessageService
    {
        Task CreateAsync(string content, string authorId, string receiverId);
    }
}
