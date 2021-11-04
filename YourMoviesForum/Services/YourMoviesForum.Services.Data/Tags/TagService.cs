using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

using YourMoviesForum.Services.Data.Tags;

namespace YourMoviesForum.Services.Data
{
    public class TagService : ITagService
    {
        private readonly YourMoviesDbContext data;

        public TagService(YourMoviesDbContext data)
        {
            this.data = data;
        }
        public async Task<bool> IsExistingAsync(string id)
            => await data.Tags.AnyAsync(t => t.Id == id && !t.IsDeleted);
    }
}
