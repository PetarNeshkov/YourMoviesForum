using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<bool> IsExistingAsync(int id)
            => await data.Tags.AnyAsync(t => t.Id == id && !t.IsDeleted);

        public Task<IEnumerable<TModel>> GetAllDataAsync<TModel>()
        {
            throw new System.NotImplementedException();
        }
    }
}
