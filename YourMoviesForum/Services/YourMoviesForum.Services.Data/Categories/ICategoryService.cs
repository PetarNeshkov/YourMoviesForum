using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Categories
{
    public interface ICategoryService
    {
        Task<IEnumerable<TModel>> GetAllCategoriesAsync<TModel>();

        Task<IEnumerable<TModel>> GetAllCategoriesAsync<TModel>(string searchFilter = null, int skip = 0, int take = 0);

        Task<int> GetPostsSearchCountAsync(string searchFilter = null);

        Task<TModel> GetCategoryByIdAsync<TModel>(int id);

        Task<bool> IsExistingAsync(string name);

        Task CreateAsync(string name);
    }
}
