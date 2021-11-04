using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Posts
{
    public interface IPostService
    {
        Task<int> CreateAsync(string title, string content, string ImageUrl, string categoryId, IEnumerable<string> tagIds);
    }
}
