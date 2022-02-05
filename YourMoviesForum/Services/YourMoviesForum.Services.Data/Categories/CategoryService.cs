using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using AutoMapper;
using AutoMapper.QueryableExtensions;

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
    }
}
