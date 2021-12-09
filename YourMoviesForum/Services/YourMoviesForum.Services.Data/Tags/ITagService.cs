using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Tags
{
    public interface ITagService
    {
        Task<IEnumerable<TModel>> GetAllTagsAsync<TModel>(string searchFilter=null,int skip=0,int take=0);

        Task<bool> IsExistingAsync(string name);

        Task<bool> IsExistingAsync(int id);

        Task CreateAsync(string name);

        Task DeleteAsync(int id);

        Task<IEnumerable<TModel>> GetAllPostTagsAsync<TModel>(int postId);

        Task<int> GetPostsSearchCountAsync(string searchFilter = null);

    }
}
