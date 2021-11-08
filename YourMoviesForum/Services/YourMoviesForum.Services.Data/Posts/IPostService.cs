using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data
{
    public interface IPostService
    {
        Task<int> CreateAsync(string title, string content, string ImageUrl, int categoryId,IEnumerable<int> tagIds);
    }
}
