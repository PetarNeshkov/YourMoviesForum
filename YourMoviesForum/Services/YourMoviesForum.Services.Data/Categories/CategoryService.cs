using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using YourMoviesForum.Data.Models;

namespace YourMoviesForum.Services.Data.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly YourMoviesDbContext data;
        private readonly IMapper mapper;

        public CategoryService(YourMoviesDbContext data,IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TModel>> GetAllCategoriesAsync<TModel>()
            => await data.Categories
                 .AsNoTracking()
                 .Where(c => !c.IsDeleted)
                 .ProjectTo<TModel>(mapper.ConfigurationProvider)
                 .ToListAsync();

        public async Task<IEnumerable<TModel>> GetAllCategoriesAsync<TModel>(string searchFilter = null, int skip = 0, int take = 0)
        {
            var queryableCategories = data.Categories
                  .Where(t => !t.IsDeleted)
                  .AsNoTracking();

            queryableCategories = SortingBySearch(searchFilter, queryableCategories);

            var categories = await queryableCategories
                .Skip(skip).Take(take)
                .ProjectTo<TModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            return categories;
        }

        private static IQueryable<Category> SortingBySearch(string searchFilter, IQueryable<Category> queryableCategories)
        {
            if (!string.IsNullOrWhiteSpace(searchFilter))
            {
                queryableCategories = queryableCategories
                    .Where(t => t.Name.ToLower().Contains(searchFilter.ToLower()));
            }

            return queryableCategories;
        }

        public async Task<int> GetPostsSearchCountAsync(string searchFilter = null)
        {
            var queryableCategories = data.Tags.Where(t => !t.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchFilter))
            {
                queryableCategories = queryableCategories
                     .Where(t => t.Name.ToLower().Contains(searchFilter.ToLower()));
            }

            var count = await queryableCategories.CountAsync();

            return count;
        }
    }
}
