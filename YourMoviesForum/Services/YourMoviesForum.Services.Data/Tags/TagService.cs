using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using YourMoviesForum.Data.Models;

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


        public async Task<IEnumerable<TModel>> GetAllTagsAsync<TModel>(string searchFilter = null,
            int skip = 0, int take = 0)
        {
            var queryableTags = data.Tags
                 .Where(t => !t.IsDeleted)
                 .AsNoTracking();

            queryableTags = SortingBySearch(searchFilter, queryableTags);

            var tags = await queryableTags
                .ProjectTo<TModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            return tags;
        }

        private static IQueryable<Tag> SortingBySearch(string searchFilter, IQueryable<Tag> queryableTags)
        {
            if (!string.IsNullOrWhiteSpace(searchFilter))
            {
                queryableTags = queryableTags
                    .Where(t => t.Name.ToLower().Contains(searchFilter.ToLower()));
            }

            return queryableTags;
        }

        public Task<IEnumerable<TModel>> GetAllPostTagsAsync<TModel>(int postId)
        {
            return null;
        }
      
        public async Task<bool> IsExistingAsync(int id)
            => await data.Tags.AnyAsync(t => t.Id == id && !t.IsDeleted);

        public async Task<int> GetPostsSearchCountAsync(string searchFilter = null)
        {
            var queryableTags = data.Tags.Where(t => !t.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchFilter))
            {
                queryableTags = queryableTags
                     .Where(t => t.Name.ToLower().Contains(searchFilter.ToLower()));
            }

            var count = await queryableTags.CountAsync();

            return count;
        }

       
    }
}
