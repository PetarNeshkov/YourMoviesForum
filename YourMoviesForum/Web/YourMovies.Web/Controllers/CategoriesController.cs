using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

using YourMovies.Web.Views.Categories;
using YourMoviesForum.Web.InputModels.Categories;
using YourMoviesForum.Services.Data;
using YourMoviesForum.Services.Data.Categories;
using YourMoviesForum.Services.Providers.Pagination;


using static YourMoviesForum.Common.ErrorMessages.Categories;
using static YourMoviesForum.Common.GlobalConstants;
using static YourMoviesForum.Common.GlobalConstants.Category;
using static YourMoviesForum.Common.GlobalConstants.Post;
using YourMoviesForum.Web.InputModels.Home;

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
            var skip = (page - 1) * CategoriesPerPage;
            var count = await categoryService.GetPostsSearchCountAsync(query.SearchTerm);
            var categories = await categoryService.GetAllCategoriesAsync<CategoryListingViewModel>(query.SearchTerm, skip, CategoriesPerPage);

            query.Pagination = PaginationProvider.PaginationHelper(page, count, CategoriesPerPage, query.SearchTerm);      
            query.Categories = categories;

            return View(query);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id,int page=1)
        {
            var category = await categoryService.GetCategoryByIdAsync<CategoryListingViewModel>(id);

            if (category==null)
            {
                return NotFound();
            }

            var skip=(page - 1) * PostPerPage;
            var posts = await postService.GetAllPostsByCategoryIdAsync<PostListingViewModel>(id, skip, PostPerPage);

            foreach (var post in posts)
            {
                post.Activity = await postService.GetLatestPostActivityAsync(post.Id);
            }

            var viewModel = new CategoryDetailsViewModel
            {
                Id = category.Id,
                Category = category,
                Posts = posts,
                Pagination = PaginationProvider.PaginationHelper(page, posts.Count, PostPerPage, null)
            };

            return View(viewModel);
        }

        [Authorize(Roles = Administrator.AdministratorUsername)]
        public IActionResult Create() => this.View();

        [Authorize(Roles = Administrator.AdministratorUsername)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel input)
        {
            var isExisting = await this.categoryService.IsExistingAsync(input.Name);

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            if (isExisting)
            {
                ModelState.AddModelError(input.Name, CategoryExistingNameErrorMessage);
                return this.View(input);
            }

            await this.categoryService.CreateAsync(input.Name);

            return this.RedirectToAction(nameof(All));
        }

        [Authorize(Roles = Administrator.AdministratorUsername)]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isExisting = await this.categoryService.IsExistingAsync(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.categoryService.DeleteAsync(id);

            return this.RedirectToAction(nameof(All));
        }
    }
}
