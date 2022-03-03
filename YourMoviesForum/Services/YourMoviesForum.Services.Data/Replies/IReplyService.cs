using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Replies
{
    public interface IReplyService
    {
        Task CreateReplyAsync(string content, int? parentId, int postId, string authorId);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<string> GetReplyAuthorIdAsync<TModel>(int id);

        Task EditAsync(int id, string content);

        Task<IEnumerable<TModel>> GetAllRepliesByPostIdAsync<TModel>(int postId);

        Task DeleteAsync(int id);
    }
}
