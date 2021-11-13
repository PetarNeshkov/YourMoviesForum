using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data
{
    public interface IPostService
    {
        Task<int> CreatePostAsync(string title, string content, string ImageUrl, int categoryId,IEnumerable<int> tagIds);

        Task<IEnumerable<TModel>> GetAllPostsAsync<TModel>();

        IEnumerable<TModel> GetThreeRandomPosts<TModel>();
    }
}
