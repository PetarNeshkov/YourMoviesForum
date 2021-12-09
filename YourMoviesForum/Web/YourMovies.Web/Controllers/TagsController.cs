using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using YourMoviesForum.Services.Data;
using YourMoviesForum.Services.Data.Tags;
using YourMoviesForum.Web.InputModels.Tags;

using static YourMoviesForum.Common.GlobalConstants;

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
            query.CurrentPage = page;
            var skip = (page - 1) * TagsPerPage;
            var count = await tagService.GetPostsSearchCountAsync(query.SearchTerm);
            var tags = await tagService.GetAllTagsAsync<TagsListingViewModel>(query.SearchTerm,skip,TagsPerPage);

            query.TotalPages= (int)Math.Ceiling(count / (decimal)TagsPerPage);
            query.Tags = tags;

            return View(query);
        }

        public IActionResult Create() => this.View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateTagInputModel input)
        {
            var isExisting = await this.tagService.IsExistingAsync(input.Name);

            if (!this.ModelState.IsValid || isExisting)
            {
                return this.View(input);
            }

            await this.tagService.CreateAsync(input.Name);

            return this.RedirectToAction(nameof(All));
        }

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
