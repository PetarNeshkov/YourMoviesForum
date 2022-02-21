using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using YourMovies.Web.Views.Categories;
using YourMoviesForum.Web.InputModels.Categories;
using YourMoviesForum.Services.Data;
using YourMoviesForum.Services.Data.Categories;
using YourMoviesForum.Services.Providers.Pagination;


using static YourMoviesForum.Common.ErrorMessages.Categories;
using static YourMoviesForum.Common.GlobalConstants;

namespace YourMovies.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IPostService postService;

        public CategoriesController(ICategoryService categoryService, IPostService postService)
        {
            this.categoryService = categoryService;
            this.postService = postService;
        }

        public async Task<IActionResult> All([FromQuery] AllCategoriesQueryModel query,int page=1)
        {
            var skip = (page - 1) * TagsPerPage;
            var count = await categoryService.GetPostsSearchCountAsync(query.SearchTerm);
            var categories = await categoryService.GetAllCategoriesAsync<CategoriesListingViewModel>(query.SearchTerm, skip, TagsPerPage);

            query.Pagination = PaginationProvider.PaginationHelper(page, count, TagsPerPage);
            query.Categories = categories;

            return View(query);
        }
    }
}
