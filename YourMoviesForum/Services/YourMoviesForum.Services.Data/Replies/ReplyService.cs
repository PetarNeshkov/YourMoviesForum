using System.Threading.Tasks;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Data.Users;

namespace YourMoviesForum.Services.Data.Replies
{
    public class ReplyService : IReplyService
    {
        private readonly YourMoviesDbContext data;
        private readonly IUserService userService;

        public ReplyService(YourMoviesDbContext data,IUserService userService)
        {
            this.data = data;
            this.userService = userService;
        }

        public async Task CreateReplyAsync(string content, int? parentId, int postId, string authorId)
        {
            var reply = new Reply
            {
                Content = content,
                ParentId = parentId,
                PostId = postId,
                AuthorId = authorId
            };

            await this.userService.AddRatingToUserAsync(authorId);

            await data.Replies.AddAsync(reply);
            await data.SaveChangesAsync();
        }
    }
}
