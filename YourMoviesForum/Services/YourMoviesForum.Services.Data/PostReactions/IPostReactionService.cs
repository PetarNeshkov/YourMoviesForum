using System.Threading.Tasks;

using YourMoviesForum.Web.InputModels.Reactions;
using YourMoviesForum.Web.InputModels.Reactions.enums;

namespace YourMoviesForum.Services.Data.PostReactions
{
    public interface IPostReactionService
    {
        Task<ReactionCountServiceModel> ReactAsync(ReactionType reactionType, int postId, string authorId);
    }
}
