using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace YourMoviesForum.Services.Data.Users
{
    public class User : IUserService
    {
        private readonly YourMoviesDbContext data;
        private readonly IMapper mapper;

        public User(YourMoviesDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<bool> IsUsernameUsedAsync(string username)
             => await data.Users.AnyAsync(u => u.UserName == username && !u.IsDeleted);
        public async Task<bool> IsEmailUsedAsync(string email)
            => await data.Users.AnyAsync(u=>u.Email==email && !u.IsDeleted);

        public async Task<int> AddRatingToUserAsync(string id, int points = 1)
        {
           var user= await data.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

           user.Rating += points;

           await data.SaveChangesAsync();

            return user.Rating;
        }

        public async Task<IEnumerable<TModel>> GetAllUsersAsync<TModel>()
             => await data.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<TModel> GetUserByIdAsync<TModel>(string id)
            => await data.Users
                .AsNoTracking()
                .Where(u => u.Id == id && !u.IsDeleted)
                .ProjectTo<TModel>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<string> GetUserBackGroundColorAsync(string id)
            =>  await data.Users
                  .AsNoTracking()
                  .Where(u => u.Id == id)
                  .Select(u => u.BackgroundColor)
                  .FirstOrDefaultAsync();

        public async Task<char> GetUserFirstLetterAsync(string id)
            => await data.Users
                   .AsNoTracking()
                   .Where(x => x.Id == id)
                   .Select(x => x.FirstLetter)
                   .FirstOrDefaultAsync();
    }
}
