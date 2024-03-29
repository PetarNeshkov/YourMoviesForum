﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

using YourMoviesForum.Services.Data.Replies;
using YourMoviesForum.Services.Data.Users;
using YourMoviesForum.Web.InputModels.Replies;

using static YourMoviesForum.Common.GlobalConstants;

namespace YourMovies.Web.Controllers
{
    [Authorize]
    public class RepliesController : Controller
    {
        private readonly IReplyService replyService;
        private readonly IUserService userService;

        public RepliesController(IReplyService replyService,IUserService userService)
        {
            this.replyService = replyService;
            this.userService = userService;
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

        [HttpPost]
        public async Task<IActionResult> Edit(EditReplyFormModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var replyAuthorId = await replyService.GetReplyAuthorIdAsync<EditReplyFormModel>(input.Id);
            if (replyAuthorId!=User.Id() && User.IsAdministrator())
            {
                return Unauthorized();
            }

            await replyService.EditAsync(input.Id, input.SanitizedContent);

            TempData[GlobalMessageKey] = $"Your reply was successfully edited!";

            return RedirectToAction(nameof(Details), new { id = input.Id});
        }

        public async Task<IActionResult> Details(int id)
        {
            var reply=await replyService.GetByIdAsync<ReplyDetailsViewModel>(id);

            if (reply==null)
            {
                return NotFound();
            }

            reply.FirstLetter = await userService.GetUserFirstLetterAsync(User.Id());
            reply.BackgroundColor = await userService.GetUserBackGroundColorAsync(User.Id());
            reply.Replies = await replyService.GetAllRepliesByPostIdAsync<ReplyDetailsViewModel>(reply.PostId);

            foreach (var item in reply.Replies)
            {
                item.BackgroundColor = await userService.GetUserBackGroundColorAsync(item.Author.Id);
                item.FirstLetter=await userService.GetUserFirstLetterAsync(item.Author.Id);
            }

            return View(reply);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reply = await replyService.GetByIdAsync<ReplyDeleteViewModel>(id);

            if (reply == null)
            {
                return NotFound();
            }

            if (reply.Author.Id != User.Id() && User.IsAdministrator())
            {
                return Unauthorized();
            }

            reply.FirstLetter = await userService.GetUserFirstLetterAsync(User.Id());
            reply.BackgroundColor = await userService.GetUserBackGroundColorAsync(User.Id());

            return View(reply);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var reply = await replyService.GetByIdAsync<ReplyDeleteAuthorViewModel>(id);

            if (reply == null)
            {
                return NotFound();
            }

            if (reply.AuthorId != User.Id() && User.IsAdministrator())
            {
                return Unauthorized();
            }

            await replyService.DeleteAsync(id);

            TempData[GlobalMessageKey] = $"Your reply was successfully deleted!";

            return RedirectToAction("Details", "Posts", new { id = reply.PostId });
        }
    }
}
