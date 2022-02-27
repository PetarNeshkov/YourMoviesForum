using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Data.Users;

namespace YourMoviesForum.Services.Data.Replies
{
    public class ReplyService : IReplyService
    {
        private readonly YourMoviesDbContext data;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public ReplyService(YourMoviesDbContext data,IUserService userService,IMapper mapper)
        {
            this.data = data;
            this.userService = userService;
            this.mapper = mapper;
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

        public async Task<TModel> GetByIdAsync<TModel>(int id)
           => await data.Replies
                .AsNoTracking()
                .Where(r => r.Id == id && !r.IsDeleted)
                .ProjectTo<TModel>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
    }
}
