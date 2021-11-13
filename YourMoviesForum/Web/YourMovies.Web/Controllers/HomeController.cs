using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using YourMoviesForum;
using YourMovies.Web.Models;
using System.Linq;
using YourMoviesForum.Services.Data;
using YourMoviesForum.Web.InputModels.Home;
using System;

namespace YourMovies.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly YourMoviesDbContext data;
        private readonly IPostService postservice;
        public HomeController(YourMoviesDbContext data,IPostService postservice)
        {
            this.data = data;
            this.postservice = postservice;
        }

        public IActionResult Index()
        {
            var totalPosts = data.Posts
                            .Where(x => !x.IsDeleted)
                            .Count();

            var posts = postservice.GetThreeRandomPosts<PostListingViewModel>();

            var viewModel = new IndexViewModel
            {
                TotalPosts=totalPosts,
                Posts= posts
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
