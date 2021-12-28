using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using YourMovies.Web.Models;
using YourMovies.Web.Views.Pagination;
using YourMoviesForum;
using YourMoviesForum.Services.Data;
using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.Posts;

using static YourMoviesForum.Common.GlobalConstants;

namespace YourMovies.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly YourMoviesDbContext data;
        private readonly IPostService postservice;
        public HomeController(YourMoviesDbContext data, IPostService postservice)
        {
            this.data = data;
            this.postservice = postservice;
        }

        public async Task<IActionResult> Index([FromQuery] AllPostsQueryModel query,int page=1)
        {

            //var totalPosts = data.Posts
            //                .Where(x => !x.IsDeleted)
            //                .Count();

            if (User.Identity.IsAuthenticated)
            { 
                var skip = (page - 1) * PostPerPage;
                var count = await postservice.GetPostsSearchCountAsync(query.SearchTerm);
                var posts = await postservice
                        .GetAllPostsAsync<PostListingViewModel>(query.Sorting,query.SearchTerm,skip,PostPerPage);

                var pagination = new PaginationViewModel
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(count / (decimal)PostPerPage)
                };

                query.Pagination = pagination;
                query.Posts = posts;
            }
            else
            {
                var posts = await postservice.GetThreeRandomPosts<PostListingViewModel>();
                query.Posts = posts;
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
