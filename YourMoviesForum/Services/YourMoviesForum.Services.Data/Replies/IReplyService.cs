using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Replies
{
    public interface IReplyService
    {
        Task CreateReplyAsync(string content, int? parentId, int postId, string authorId);
    }
}
