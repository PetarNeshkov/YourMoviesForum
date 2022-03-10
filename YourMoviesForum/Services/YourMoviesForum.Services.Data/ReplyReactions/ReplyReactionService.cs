using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Web.InputModels.Reactions;
using YourMoviesForum.Web.InputModels.Reactions.enums;

namespace YourMoviesForum.Services.Data.ReplyReactions
{
    public class ReplyReactionService : IReplyReactionService
    {
        private readonly YourMoviesDbContext data;
        public ReplyReactionService(YourMoviesDbContext data)
        {
            this.data = data;
        }

        public async Task<ReactionCountServiceModel> ReactAsync(ReactionType reactionType, int replyId, string authorId)
        {
            var reaction = await data.ReplyReactions
                 .FirstOrDefaultAsync(r => r.ReplyId == replyId && r.AuthorId == authorId);

            if (reaction == null)
            {
                reaction = new ReplyReaction
                {
                    ReactionType = reactionType,
                    ReplyId = replyId,
                    AuthorId = authorId,
                };

                await data.ReplyReactions.AddAsync(reaction);
            }
            else
            {
                reaction.ReactionType = reactionType;
            }

            await data.SaveChangesAsync();

            return await GetReactionsCountByPostIdAsync(replyId);
        }

        private async Task<ReactionCountServiceModel> GetReactionsCountByPostIdAsync(int postId)
           => new ReactionCountServiceModel
           {
               Likes = await GetCountByPostTypeAndIdAsync(ReactionType.Like, postId),
               HeartReactionsCount = await GetCountByPostTypeAndIdAsync(ReactionType.Heart, postId),
               HahaReactionsCount = await GetCountByPostTypeAndIdAsync(ReactionType.Haha, postId),
               WowReactionsCount = await GetCountByPostTypeAndIdAsync(ReactionType.Wow, postId),
               SadReactionsCount = await GetCountByPostTypeAndIdAsync(ReactionType.Sad, postId),
               AngryReactionsCount = await GetCountByPostTypeAndIdAsync(ReactionType.Angry, postId)
           };

        private async Task<int> GetCountByPostTypeAndIdAsync(ReactionType reactionType, int postId)
            => await data.PostReactions
                .Where(r => r.PostId == postId && !r.Post.IsDeleted)
                .CountAsync(pr => pr.ReactionType == reactionType);
    }
}
