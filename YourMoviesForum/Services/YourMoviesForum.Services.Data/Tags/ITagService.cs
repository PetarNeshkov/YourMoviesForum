using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Tags
{
    public interface ITagService
    {
        Task<IEnumerable<TModel>> GetAllTagsAsync<TModel>();

        Task<bool> IsExistingAsync(int id);

        Task<IEnumerable<TModel>> GetAllPostTagsAsync<TModel>(int postId);

    }
}
