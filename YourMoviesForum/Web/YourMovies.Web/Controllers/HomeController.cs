using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using YourMoviesForum.Services.Data;
using YourMoviesForum.Services.Providers.Pagination;
using YourMovies.Web.Models;
using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.Posts;

using static YourMoviesForum.Common.GlobalConstants.Post;
using static YourMoviesForum.Common.GlobalConstants;
using System.Collections.Generic;
using System;

namespace YourMovies.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IPostService postservice;
        private readonly IMemoryCache cache;
        public HomeController(IPostService postservice,IMemoryCache cache)
        {
            this.postservice = postservice;
            this.cache = cache;
        }
       
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("All", "Posts");
            }

            var randomPosts = cache.Get<IEnumerable<PostListingViewModel>>(Cache.LatestPostsCacheKey);

            if (randomPosts==null)
            {
                 randomPosts = await postservice
                    .GetFourRandomPosts<PostListingViewModel>();

                var cacheOptions = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                this.cache.Set(Cache.LatestPostsCacheKey, randomPosts, cacheOptions);
            }

            return View(randomPosts);
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
