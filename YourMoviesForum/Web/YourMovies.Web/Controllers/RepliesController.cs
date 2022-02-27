using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourMovies.Web.Infrastructure;
using YourMoviesForum.Services.Data.Replies;
using YourMoviesForum.Web.InputModels.Replies;

namespace YourMovies.Web.Controllers
{
    public class RepliesController : Controller
    {
        private readonly IReplyService replyService;

        public RepliesController(IReplyService replyService)
        {
            this.replyService = replyService;
        }

        public async Task<IActionResult> Create(ReplyCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Details", "Posts", new { id = input.PostId });
            }

            await replyService.CreateReplyAsync(input.Content, input.ParentId, input.PostId, User.Id());

            return RedirectToAction("Details", "Posts", new { id = input.PostId });
        }
    }
}
