using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using YourMoviesForum.Services.Data.ReplyReactions;
using YourMoviesForum.Web.InputModels.Reactions;
using YourMoviesForum.Web.InputModels.Reactions.enums;

namespace YourMovies.Web.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("/api/reply-reactions")]
    public class ReplyReactionsController : ControllerBase
    {
        private readonly IReplyReactionService replyReactionService;

        public ReplyReactionsController(IReplyReactionService replyReactionService)
        {
            this.replyReactionService = replyReactionService;
        }

        [HttpPost("like/{replyId}")]
        public async Task<ActionResult<ReactionCountServiceModel>> Like(int replyId)
           => await replyReactionService.ReactAsync(
               ReactionType.Like,
               replyId,
               User.Id());

        [HttpPost("heart/{replyId}")]
        public async Task<ActionResult<ReactionCountServiceModel>> Love(int replyId)
            => await replyReactionService.ReactAsync(
                ReactionType.Heart,
                replyId,
                User.Id());

        [HttpPost("haha/{replyId}")]
        public async Task<ActionResult<ReactionCountServiceModel>> Haha(int replyId)
            => await replyReactionService.ReactAsync(
                ReactionType.Haha,
                replyId,
                User.Id());

        [HttpPost("wow/{replyId}")]
        public async Task<ActionResult<ReactionCountServiceModel>> Wow(int replyId)
            => await replyReactionService.ReactAsync(
                ReactionType.Wow,
                replyId,
                User.Id());

        [HttpPost("sad/{replyId}")]
        public async Task<ActionResult<ReactionCountServiceModel>> Sad(int replyId)
            => await replyReactionService.ReactAsync(
                ReactionType.Sad,
                replyId,
                User.Id());

        [HttpPost("angry/{replyId}")]
        public async Task<ActionResult<ReactionCountServiceModel>> Angry(int replyId)
            => await replyReactionService.ReactAsync(
                ReactionType.Angry,
                replyId,
                User.Id());
    }
}
