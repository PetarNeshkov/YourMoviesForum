using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Web.InputModels.Reactions;
using YourMoviesForum.Web.InputModels.Reactions.enums;

namespace YourMoviesForum.Services.Data.PostReactions
{
    public class PostReactionService : IPostReactionService
    {
        private readonly YourMoviesDbContext data;

        public PostReactionService(YourMoviesDbContext data)
        { 
            this.data = data;           
        }

        public async Task<ReactionCountServiceModel> ReactAsync(ReactionType reactionType, int postId, string authorId)
        {
            var reaction = await data.PostReactions
                .FirstOrDefaultAsync(r => r.PostId == postId && r.AuthorId == authorId);

            if (reaction == null)
            {
                reaction = new PostReaction
                {
                    ReactionType = reactionType,
                    PostId = postId,
                    AuthorId = authorId,
                };

                await data.PostReactions.AddAsync(reaction);
            }
            else
            {
                reaction.ReactionType = reactionType;
            }

            await data.SaveChangesAsync();

            return await GetReactionsCountByPostIdAsync(postId);
        }

        private async Task<ReactionCountServiceModel> GetReactionsCountByPostIdAsync(int postId)
            => new ReactionCountServiceModel
            {
                Likes=await GetCountByPostTypeAndIdAsync(ReactionType.Like, postId),
                Dislikes=await GetCountByPostTypeAndIdAsync(ReactionType.Dislike, postId),
            };

        private async Task<int> GetCountByPostTypeAndIdAsync(ReactionType reactionType,int postId)
            =>await data.PostReactions
                .Where(r=>r.PostId==postId && !r.Post.IsDeleted)
                .CountAsync(pr => pr.ReactionType == reactionType);
    }
}
