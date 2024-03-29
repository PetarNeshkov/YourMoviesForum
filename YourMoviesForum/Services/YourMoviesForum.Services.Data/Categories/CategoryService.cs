﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Providers.DateTime;

namespace YourMoviesForum.Services.Data.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly YourMoviesDbContext data;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public CategoryService(YourMoviesDbContext data,IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.data = data;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
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
                  .Where(c => !c.IsDeleted)
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
            var queryableCategories = data.Categories.Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchFilter))
            {
                queryableCategories = queryableCategories
                     .Where(c => c.Name.ToLower().Contains(searchFilter.ToLower()));
            }

            var count = await queryableCategories.CountAsync();

            return count;
        }

        public async Task<TModel> GetCategoryByIdAsync<TModel>(int id)
            => await data.Categories
                   .AsNoTracking()
                   .Where(c => c.Id == id && !c.IsDeleted)
                   .ProjectTo<TModel>(mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync();

        public async Task<bool> IsExistingAsync(string name)
          => await data.Categories.AnyAsync(c => c.Name == name && !c.IsDeleted);
        public async Task<bool> IsExistingAsync(int id)
            =>await data.Categories.AnyAsync(c => c.Id == id && !c.IsDeleted);
        public async Task CreateAsync(string name)
        {
            var category = new Category
            {
                Name = name
            };

            await data.Categories.AddAsync(category);
            await data.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await data.Categories.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            category.IsDeleted = true;
            category.DeletedOn = dateTimeProvider.Now();

            await data.SaveChangesAsync();
        }
    }
}
