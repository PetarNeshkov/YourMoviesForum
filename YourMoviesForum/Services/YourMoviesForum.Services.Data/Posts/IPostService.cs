using System.Collections.Generic;
using System.Threading.Tasks;

using YourMoviesForum.Web.InputModels.Posts;
namespace YourMoviesForum.Services.Data
{
    public interface IPostService
    {
        Task<int> CreatePostAsync(string title, string content,int categoryId,IEnumerable<int> tagIds);

        Task<IEnumerable<TModel>> GetAllPostsAsync<TModel>(AllPostsQueryModel query, 
            string searchFilter = null, int skip = 0,
            int take = 0);

        Task<IEnumerable<TModel>> GetThreeRandomPosts<TModel>();

        Task<int> GetPostsSearchCountAsync(string searchFilter = null);
    }
}
