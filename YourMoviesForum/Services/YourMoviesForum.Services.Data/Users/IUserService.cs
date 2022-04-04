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

        Task<TModel> GetUserByIdAsync<TModel>(string id);

        Task<string> GetUserBackGroundColorAsync(string id);

        Task<char> GetUserFirstLetterAsync(string id);

    }
}
