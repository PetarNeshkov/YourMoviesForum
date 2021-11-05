using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data
{
    public interface IPostService
    {
        Task<string> CreateAsync(string title, string content, string ImageUrl, string categoryId);
    }
}
