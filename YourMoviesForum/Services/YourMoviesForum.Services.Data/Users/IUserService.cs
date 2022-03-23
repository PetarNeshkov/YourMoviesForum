using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourMoviesForum.Services.Data.Users
{
    public interface IUserService
    {
        Task<bool> IsUsernameUsedAsync(string username);

        Task<bool> IsEmailUsedAsync(string email);

        Task<int> AddRatingToUserAsync(string id, int points = 1);

        Task<IEnumerable<TModel>> GetAllUsersAsync<TModel>();
    }
}
