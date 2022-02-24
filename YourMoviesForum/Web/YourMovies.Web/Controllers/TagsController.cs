using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using YourMoviesForum.Services.Data;
using YourMoviesForum.Services.Data.Tags;
using YourMoviesForum.Services.Providers.Pagination;
using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.Tags;

using static YourMoviesForum.Common.ErrorMessages.Tags;
using static YourMoviesForum.Common.GlobalConstants;
using static YourMoviesForum.Common.GlobalConstants.Post;
using static YourMoviesForum.Common.GlobalConstants.Tag;

namespace YourMovies.Web.Controllers
{
    public class TagsController:Controller
    {
        private readonly ITagService tagService;
        private readonly IPostService postService;

        public TagsController(ITagService tagService,IPostService postService)
        {
            this.tagService = tagService;
            this.postService = postService;
        }

        public async Task<IActionResult> All([FromQuery]AllTagsQueryModel query,int page = 1)
        {
          
            var skip = (page - 1) * TagsPerPage;
            var count = await tagService.GetPostsSearchCountAsync(query.SearchTerm);
            var tags = await tagService.GetAllTagsAsync<TagsListingViewModel>(query.SearchTerm,skip,TagsPerPage);

            query.Pagination = PaginationProvider.PaginationHelper(page,count,TagsPerPage,query.SearchTerm);
            query.Tags = tags;

            return View(query);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id,int page=1)
        {

            var tag = await tagService.GetTagByIdAsync<TagsListingViewModel>(id);
            if (tag==null)
            {
                return NotFound();
            }

            var skip = (page - 1) * PostPerPage;
            var posts = await postService.GetAllPostsByTagIdAsync<PostListingViewModel>(id, skip,PostPerPage);

            var viewModel = new TagDetailsViewModel
            {
                Id = tag.Id,
                Tag = tag,
                Posts = posts,
                Pagination = PaginationProvider.PaginationHelper(page, posts.Count, TagsPerPage, null)
            };

            return View(viewModel);
        }

        [Authorize(Roles =Administrator.AdministratorUsername)]
        public IActionResult Create() => this.View();

        [Authorize(Roles = Administrator.AdministratorUsername)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTagInputModel input)
        {
            var isExisting = await this.tagService.IsExistingAsync(input.Name);

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            if (isExisting)
            {
                ModelState.AddModelError(input.Name,TagExistingNameErrorMessage);
                return this.View(input);
            }

            await this.tagService.CreateAsync(input.Name);

            return this.RedirectToAction(nameof(All));
        }

        [Authorize(Roles = Administrator.AdministratorUsername)]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isExisting = await this.tagService.IsExistingAsync(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.tagService.DeleteAsync(id);

            return this.RedirectToAction(nameof(All));
        }

    }
}
