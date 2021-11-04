using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using YourMoviesForum;
using YourMoviesForum.Data.Models;
using YourMoviesForum.Web.InputModels;
using YourMoviesForum.Web.InputModels.Tags;

namespace YourMovies.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly YourMoviesDbContext data;

        public PostsController(YourMoviesDbContext data)
            => this.data = data;

        public async Task<IActionResult> Add() => View();

        [HttpPost]
        public async Task<IActionResult> Create(AddPostFormModel input)
        {
            if (!ModelState.IsValid)
            {
                input.Tags = GetPostTags();

                return View(input);
            }

            var post=new Post 
            { 

            }
        }

        private IEnumerable<PostsTagViewModel> GetPostTags()
           => this.data
               .Categories
               .Select(c => new PostsTagViewModel
               {
                   Id = c.Id,
                   Name = c.Name
               })
               .ToList();

    }
}
