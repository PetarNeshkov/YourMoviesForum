using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Tags
{
    public interface ITagService
    {
        Task<IEnumerable<TModel>> GetAllTagsAsync<TModel>(string searchFilter=null,int skip=0,int take=0);

        Task<IEnumerable<TModel>> GetAllTagsAsync<TModel>();

        Task<bool> IsExistingAsync(string name);

        Task<bool> IsExistingAsync(int id);

        Task CreateAsync(string name);

        Task DeleteAsync(int id);

        Task<TModel> GetTagByIdAsync<TModel>(int id);

        Task<int> GetPostsSearchCountAsync(string searchFilter = null);

    }
}
