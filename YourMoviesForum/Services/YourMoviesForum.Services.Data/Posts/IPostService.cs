using System.Collections.Generic;
using System.Threading.Tasks;

using YourMoviesForum.Web.InputModels.Posts;
namespace YourMoviesForum.Services.Data
{
    public interface IPostService
    {
        Task<int> CreatePostAsync(string title, string content,int categoryId,IEnumerable<int> tagIds,string authorId);

        Task<IEnumerable<TModel>> GetAllPostsAsync<TModel>(PostSorting query, 
            string searchFilter = null, int skip = 0,
            int take = 0);

        Task<IEnumerable<TModel>> GetThreeRandomPosts<TModel>();

        Task<int> GetPostsSearchCountAsync(string searchFilter = null);

        Task<IEnumerable<TModel>> GetAllPostsByTagIdAsync<TModel>(int tagId, string search = null);

        Task<IEnumerable<TModel>> GetAllPostsByTagIdAsync<TModel>(int tagId, int skip = 0,
            int take = 0);

        Task<TModel> GetByIdAsync<TModel>(int id);

        Task<string> GetPostAuthorIdAsync<TModel>(int id);

        Task EditPostAsync(int id, string title, string content, int categoryId, IEnumerable<int> tagIds);
    }
}
