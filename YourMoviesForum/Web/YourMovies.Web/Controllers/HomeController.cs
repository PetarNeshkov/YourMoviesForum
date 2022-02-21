using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using YourMoviesForum.Services.Data;
using YourMoviesForum.Services.Providers.Pagination;
using YourMovies.Web.Models;
using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.Posts;

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

        
        [ResponseCache(Duration =700)]
        public async Task<IActionResult> Index([FromQuery] AllPostsQueryModel query,int page=1)
        {
      
                var count = await postservice.GetPostsSearchCountAsync(query.SearchTerm);

                if (User.Identity.IsAuthenticated)
                {
                    var skip = (page - 1) * PostPerPage;
                    var posts = await postservice
                            .GetAllPostsAsync<PostListingViewModel>(query.Sorting, query.SearchTerm, skip, PostPerPage);
                    query.Posts = posts;
                    query.Pagination = PaginationProvider.PaginationHelper(page, count,PostPerPage);
                }
                else
                {
                    var posts = await postservice.GetFourRandomPosts<PostListingViewModel>();
                    query.Posts = posts;
                    query.Pagination = PaginationProvider.PaginationHelper(page, 3, PostPerPage);
                }

            return View(query);
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
