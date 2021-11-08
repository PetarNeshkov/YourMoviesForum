using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Tags
{
    public interface ITagService
    {
        Task<bool> IsExistingAsync(int id);

        Task<IEnumerable<TModel>> GetAllDataAsync<TModel>();
    }
}
