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

        private async Task<ReactionCountServiceModel> GetReactionsCountByPostIdAsync(int replyId)
           => new ReactionCountServiceModel
           {
               Likes = await GetCountByReplyTypeAndIdAsync(ReactionType.Like, replyId),
               HeartReactionsCount = await GetCountByReplyTypeAndIdAsync(ReactionType.Heart, replyId),
               HahaReactionsCount = await GetCountByReplyTypeAndIdAsync(ReactionType.Haha, replyId),
               WowReactionsCount = await GetCountByReplyTypeAndIdAsync(ReactionType.Wow, replyId),
               SadReactionsCount = await GetCountByReplyTypeAndIdAsync(ReactionType.Sad, replyId),
               AngryReactionsCount = await GetCountByReplyTypeAndIdAsync(ReactionType.Angry, replyId)
           };

        private async Task<int> GetCountByReplyTypeAndIdAsync(ReactionType reactionType, int replyId)
            => await data.ReplyReactions
                .Where(r => r.ReplyId == replyId && !r.Reply.IsDeleted)
                .CountAsync(pr => pr.ReactionType == reactionType);
    }
}
