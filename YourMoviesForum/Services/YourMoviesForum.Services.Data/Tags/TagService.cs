using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Tags
{
    public class TagService : ITagService
    {
        private readonly YourMoviesDbContext data;

        public TagService(YourMoviesDbContext data)
        {
            this.data = data;
        }
        public async Task<bool> IsExistingAsync(int id)
            => await data.Tags.AnyAsync(t => t.Id == id && !t.IsDeleted);
    }
}
