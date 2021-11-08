using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourMoviesForum;
using YourMoviesForum.Services.Data;
using YourMoviesForum.Web.InputModels;
using YourMoviesForum.Web.InputModels.Posts;
using YourMoviesForum.Web.InputModels.Tags;

namespace YourMovies.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly YourMoviesDbContext data;
        private readonly IPostService postService;

        public PostsController(
            YourMoviesDbContext data,
            IPostService postService)
        {
            this.postService = postService;
            this.data = data;
        }

        public async Task<IActionResult> Add() => View(new AddPostFormModel
        {
            Tags = GetPostTags(),
            Categories = GetPostCategories()
        });
       
        [HttpPost]
        public async Task<IActionResult> Create(AddPostFormModel input)
        {
            if (!ModelState.IsValid)
            {
                input.Tags = GetPostTags();

                return View(input);
            }

            var post = await postService.CreateAsync(
                input.Title,
                input.ImageUrl,
                input.Content,
                input.CategoryId);


            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<PostsTagViewModel> GetPostTags()
           => data
               .Tags
               .Select(c => new PostsTagViewModel
               {
                   Id = c.Id,
                   Name = c.Name
               })
               .ToList();

        private IEnumerable<PostCategoryViewModel> GetPostCategories()
            => data
                .Categories
                .Select(t => new PostCategoryViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToList();
    }
}
