using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Categories
{
    public interface ICategoryService
    {
        Task<IEnumerable<TModel>> GetAllCategoriesAsync<TModel>();
    }
}
