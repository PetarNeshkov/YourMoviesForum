using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace YourMoviesForum.Services.Data.Tags
{
    public class TagService : ITagService
    {
        private readonly YourMoviesDbContext data;
        private readonly IMapper mapper;

        public TagService(YourMoviesDbContext data,IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }


        public async Task<IEnumerable<TModel>> GetAllTagsAsync<TModel>()
        {
            var queryableTags = data.Tags
                 .Where(t => !t.IsDeleted)
                 .AsNoTracking();

            var tags = await queryableTags
                .ProjectTo<TModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            return tags;
        }

        public Task<IEnumerable<TModel>> GetAllPostTagsAsync<TModel>(int postId)
        {
            return null;
        }
       

        public async Task<bool> IsExistingAsync(int id)
            => await data.Tags.AnyAsync(t => t.Id == id && !t.IsDeleted);

    }
}
