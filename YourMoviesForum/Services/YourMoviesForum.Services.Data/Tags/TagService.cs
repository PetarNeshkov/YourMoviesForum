using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Providers.DateTime;

namespace YourMoviesForum.Services.Data.Tags
{
    public class TagService : ITagService
    {
        private readonly YourMoviesDbContext data;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public TagService(YourMoviesDbContext data,IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.data = data;
            this.mapper = mapper;
            this.dateTimeProvider=dateTimeProvider;
        }


        public async Task<IEnumerable<TModel>> GetAllTagsAsync<TModel>(string searchFilter = null,
            int skip = 0, int take = 0)
        {
            var queryableTags = data.Tags
                 .Where(t => !t.IsDeleted)
                 .AsNoTracking();

            queryableTags = SortingBySearch(searchFilter, queryableTags);

            var tags = await queryableTags
                .Skip(skip).Take(take)
                .ProjectTo<TModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            return tags;
        }

        public async Task<IEnumerable<TModel>> GetAllTagsAsync<TModel>()
                =>await data.Tags
                 .Where(t => !t.IsDeleted)
                 .AsNoTracking()
                 .ProjectTo<TModel>(mapper.ConfigurationProvider)
                 .ToListAsync();

        private static IQueryable<Tag> SortingBySearch(string searchFilter, IQueryable<Tag> queryableTags)
        {
            if (!string.IsNullOrWhiteSpace(searchFilter))
            {
                queryableTags = queryableTags
                    .Where(t => t.Name.ToLower().Contains(searchFilter.ToLower()));
            }

            return queryableTags;
        }

      
        public async Task<bool> IsExistingAsync(string name)
            => await data.Tags.AnyAsync(t => t.Name == name && !t.IsDeleted);

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

        public async Task CreateAsync(string name)
        {
            var tag = new Tag
            {
                Name = name
            };

            await data.Tags.AddAsync(tag);
            await data.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await data.Tags.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            tag.IsDeleted = true;
            tag.DeletedOn = dateTimeProvider.Now();

            await data.SaveChangesAsync();
        }

        public async Task<TModel> GetTagByIdAsync<TModel>(int id)
            => await data.Tags
                .AsNoTracking()
                .Where(t => t.Id == id && !t.IsDeleted)
                .ProjectTo<TModel>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TModel>> GetAllPostsByIdAsync<TModel>(int postId)
            => await data.Posts
                .AsNoTracking()
                .Where(p=>p.Id== postId && !p.IsDeleted)
                .SelectMany(t=>t.Tags)
                .Where(t=>!t.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
