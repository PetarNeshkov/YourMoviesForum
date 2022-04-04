using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using YourMoviesForum.Services.Data;
using YourMovies.Web.Models;
using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Services.Data.Users;

using static YourMoviesForum.Common.GlobalConstants;

namespace YourMovies.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IPostService postservice;
        private readonly IUserService userService;
        private readonly IMemoryCache cache;
        public HomeController(IPostService postservice,IUserService userService,IMemoryCache cache)
        {
            this.postservice = postservice;
            this.userService = userService;
            this.cache = cache;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("All", "Posts",new {page=page});
            }

            var randomPosts = cache.Get<IEnumerable<PostListingViewModel>>(Cache.LatestPostsCacheKey);

            if (randomPosts==null)
            {
                 randomPosts = await postservice
                    .GetFourRandomPosts<PostListingViewModel>();

                foreach (var post in randomPosts)
                {
                    post.Activity = await postservice.GetLatestPostActivityAsync(post.Id);
                    post.FirstLetter = await userService.GetUserFirstLetterAsync(post.AuthorId);
                    post.BackgroundColor=await userService.GetUserBackGroundColorAsync(post.AuthorId);
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                 cache.Set(Cache.LatestPostsCacheKey, randomPosts, cacheOptions);
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
