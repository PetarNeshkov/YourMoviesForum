using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using YourMoviesForum;
using YourMoviesForum.Services.Data;
using YourMoviesForum.Services.Data.Categories;
using YourMoviesForum.Services.Data.Replies;
using YourMoviesForum.Services.Data.Tags;
using YourMoviesForum.Services.Data.Users;
using YourMoviesForum.Services.Providers.Pagination;
using YourMoviesForum.Web.InputModels;
using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.Posts;

using static YourMoviesForum.Common.GlobalConstants;
using static YourMoviesForum.Common.GlobalConstants.Post;

namespace YourMovies.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly YourMoviesDbContext data;
        private readonly IPostService postService;
        private readonly ICategoryService categoryService;
        private readonly ITagService tagService;
        private readonly IReplyService replyService;
        private readonly IUserService userService;

        public PostsController(
            YourMoviesDbContext data,
            IPostService postService,
            ICategoryService categoryService,
            ITagService tagService,
            IReplyService replyService,
            IUserService userService)
        {
            this.data = data;
            this.postService = postService;
            this.categoryService = categoryService;
            this.tagService = tagService;
            this.replyService = replyService;
            this.userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> All([FromQuery] AllPostsQueryModel query, int page = 1)
        {

            var count = await postService.GetPostsSearchCountAsync(query.SearchTerm);

            var skip = (page - 1) * PostPerPage;
            var posts = await postService
                    .GetAllPostsAsync<PostListingViewModel>(query.Sorting, query.SearchTerm, skip, PostPerPage);
            query.Posts = posts;
            query.Pagination = PaginationProvider.PaginationHelper(page, count, PostPerPage, query.SearchTerm);

            foreach (var post in query.Posts)
            {
                post.FirstLetter = await userService.GetUserFirstLetterAsync(post.AuthorId);
                post.BackgroundColor = await userService.GetUserBackGroundColorAsync(post.AuthorId);
                post.Activity = await postService.GetLatestPostActivityAsync(post.Id);
            }

            return View(query);
        }

        [Authorize]
        public async Task<IActionResult> Add() => View(new AddPostFormModel
        {
            Tags = await tagService.GetAllTagsAsync<PostTagViewModel>(),
            Categories = await categoryService.GetAllCategoriesAsync<PostCategoryViewModel>()
        });


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddPostFormModel input)
        {
            if (!ModelState.IsValid)
            {
                input.Tags = await tagService.GetAllTagsAsync<PostTagViewModel>();
                input.Categories = await categoryService.GetAllCategoriesAsync<PostCategoryViewModel>();

                return View(input);
            }

            var post=await postService.CreatePostAsync(
                input.Title,
                input.SanitizedContent,
                input.CategoryId,
                input.TagIds,
                User.Id());
            return RedirectToAction(nameof(Details), new {id=post});
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await postService.GetByIdAsync<PostDetailsViewModel>(id);

            if (post == null)
            {
                return NotFound();
            }

            await this.postService.ViewAsync(id);

            post.FirstLetter = await userService.GetUserFirstLetterAsync(User.Id());
            post.BackgroundColor= await userService.GetUserBackGroundColorAsync(User.Id());

            post.Tags = await tagService.GetAllPostsByIdAsync<PostTagViewModel>(id);
            post.Replies = await replyService.GetAllRepliesByPostIdAsync<PostRepliesDetailsViewModel>(id);

            foreach (var reply in post.Replies)
            {
                reply.FirstLetter = await userService.GetUserFirstLetterAsync(reply.Author.Id);
                reply.BackgroundColor = await userService.GetUserBackGroundColorAsync(reply.Author.Id);
            }

            return View(post);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await postService.GetByIdAsync<EditPostFormModel>(id);

            if (post == null)
            {
                return NotFound();
            }

            if (post.AuthorId != User.Id() && !User.IsAdministrator())
            {
                return Unauthorized();
            }

            post.Tags = await tagService.GetAllTagsAsync<PostTagViewModel>();
            post.Categories = await categoryService.GetAllCategoriesAsync<PostCategoryViewModel>();

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPostFormModel input)
        {
            if (!ModelState.IsValid)
            {
                input.Tags = await tagService.GetAllTagsAsync<PostTagViewModel>();
                input.Categories = await categoryService.GetAllCategoriesAsync<PostCategoryViewModel>();

                return View(input);
            }

            var postAuthorId = await postService.GetPostAuthorIdAsync<PostDetailsViewModel>(input.Id);

            if (postAuthorId != User.Id() && !User.IsAdministrator())
            {
                return Unauthorized();
            }

            await postService.EditPostAsync(
                input.Id,
                input.Title,
                input.SanitizedContent,
                input.CategoryId,
                input.TagIds);

            TempData[GlobalMessageKey] = $"Your post was successfully edited!";

            return RedirectToAction(nameof(Details), new { id = input.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var post = await postService.GetByIdAsync<PostDeleteViewModel>(id);

            if (post == null)
            {
                return NotFound();
            }

            if (post.Author.Id != User.Id() && User.IsAdministrator())
            {
                return Unauthorized();
            }

            post.FirstLetter = await userService.GetUserFirstLetterAsync(User.Id());
            post.BackgroundColor = await userService.GetUserBackGroundColorAsync(User.Id());
            post.Tags = await tagService.GetAllPostsByIdAsync<PostTagViewModel>(id);

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var post = await this.postService.GetByIdAsync<PostDeleteAuthorViewModel>(id);

            if (post == null)
            {
                return NotFound();
            }

            if (post.AuthorId != User.Id() && User.IsAdministrator())
            {
                return Unauthorized();
            }

            await postService.DeletePostAsync(id);

            TempData[GlobalMessageKey] = $"Your post was successfully deleted!";

            return RedirectToAction("Index", "Home");
        }
    }
}
