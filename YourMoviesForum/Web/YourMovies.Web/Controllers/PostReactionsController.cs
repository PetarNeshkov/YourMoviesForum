using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourMovies.Web.Infrastructure;

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

        [HttpPost("dislike/{postId}")]
        public async Task<ActionResult<ReactionCountServiceModel>> Dislike(int postId)
            => await postReactionService.ReactAsync(ReactionType.Dislike, postId, User.Id());
    }
}
