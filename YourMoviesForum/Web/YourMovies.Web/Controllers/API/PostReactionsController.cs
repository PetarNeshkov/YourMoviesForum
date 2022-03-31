using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using YourMoviesForum.Services.Data.PostReactions;
using YourMoviesForum.Web.InputModels.Reactions;
using YourMoviesForum.Web.InputModels.Reactions.enums;

namespace YourMovies.Web.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("api/post-reactions")]
    public class PostReactionsController : ControllerBase
    {
        private readonly IPostReactionService postReactionService;

        public PostReactionsController(
            IPostReactionService postReactionService)
        {
            this.postReactionService = postReactionService;
        }

        [HttpPost("like/{postId}")]
        public async Task<ActionResult<ReactionCountServiceModel>> Like(int postId)
             => await postReactionService.ReactAsync(ReactionType.Like, postId, User.Id());

        [HttpPost("heart/{postId}")]
        public async Task<ActionResult<ReactionCountServiceModel>> Heart(int postId)
            => await postReactionService.ReactAsync(ReactionType.Heart, postId, User.Id());

        [HttpPost("haha/{postId}")]
        public async Task<ActionResult<ReactionCountServiceModel>> Haha(int postId)
            => await postReactionService.ReactAsync(ReactionType.Haha, postId, User.Id());

        [HttpPost("wow/{postId}")]
        public async Task<ActionResult<ReactionCountServiceModel>> Wow(int postId)
            => await postReactionService.ReactAsync(ReactionType.Wow, postId, User.Id());

        [HttpPost("sad/{postId}")]
        public async Task<ActionResult<ReactionCountServiceModel>> Sad(int postId)
            => await postReactionService.ReactAsync(ReactionType.Sad, postId, User.Id());

        [HttpPost("angry/{postId}")]
        public async Task<ActionResult<ReactionCountServiceModel>> Angry(int postId)
            => await postReactionService.ReactAsync(ReactionType.Angry, postId, User.Id());
    }
}
