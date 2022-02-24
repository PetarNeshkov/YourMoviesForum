using AutoMapper;
using YourMoviesForum.Services.Providers.DateTime;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Users
{
    public class User : IUserService
    {
        private readonly YourMoviesDbContext data;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public User(YourMoviesDbContext data, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            this.data = data;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<bool> IsUsernameUsedAsync(string username)
             => await data.Users.AnyAsync(u => u.UserName == username && !u.IsDeleted);
        public async Task<bool> IsEmailUsedAsync(string email)
            => await data.Users.AnyAsync(u=>u.Email==email && !u.IsDeleted);
    }
}
