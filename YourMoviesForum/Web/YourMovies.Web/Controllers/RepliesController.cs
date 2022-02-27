using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourMovies.Web.Infrastructure;
using YourMoviesForum.Services.Data.Replies;
using YourMoviesForum.Web.InputModels.Replies;

namespace YourMovies.Web.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> Edit(int id)
        {
            var reply=await replyService.GetByIdAsync<EditReplyFormModel>(id);

            if (reply == null)
            {
                return NotFound();
            }

            if (reply.AuthorId != User.Id() && !this.User.IsAdministrator())
            {
                return Unauthorized();
            }

            return View(reply);
        }
    }
}
