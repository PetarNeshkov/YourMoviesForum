using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Tags
{
    public interface ITagService
    {
        Task<bool> IsExistingAsync(string id);
    }
}
