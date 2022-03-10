using System.Threading.Tasks;

using YourMoviesForum.Web.InputModels.Reactions;
using YourMoviesForum.Web.InputModels.Reactions.enums;

namespace YourMoviesForum.Services.Data.ReplyReactions
{
    public interface IReplyReactionService
    {
        Task<ReactionCountServiceModel> ReactAsync(ReactionType reactionType, int replyId, string authorId);
    }
}
