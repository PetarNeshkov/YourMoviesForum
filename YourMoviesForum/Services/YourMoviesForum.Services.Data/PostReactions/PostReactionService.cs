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

        private async Task<ReactionCountServiceModel> GetReactionsCountByPostIdAsync(int replyId)
            => new ReactionCountServiceModel
            {
                Likes=await GetCountByPostTypeAndIdAsync(ReactionType.Like, replyId),
                HeartReactionsCount=await GetCountByPostTypeAndIdAsync(ReactionType.Heart, replyId),
                HahaReactionsCount=await GetCountByPostTypeAndIdAsync(ReactionType.Haha, replyId),
                WowReactionsCount=await GetCountByPostTypeAndIdAsync(ReactionType.Wow, replyId),
                SadReactionsCount=await GetCountByPostTypeAndIdAsync(ReactionType.Sad, replyId),
                AngryReactionsCount=await GetCountByPostTypeAndIdAsync(ReactionType.Angry, replyId)
            };

        private async Task<int> GetCountByPostTypeAndIdAsync(ReactionType reactionType,int replyId)
            =>await data.ReplyReactions
                .Where(r=>r.Reply.Id== replyId && !r.Reply.IsDeleted)
                .CountAsync(pr => pr.ReactionType == reactionType);
    }
}
