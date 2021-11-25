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

        
        public async Task<IActionResult> All(AllTagsQueryModel query,int page = 1)
        {
            var skip = (page - 1) * TagsPerPage;
            var count = await tagService.GetPostsSearchCountAsync(query.SearchTerm);
            var tags = await tagService.GetAllTagsAsync<TagsListingViewModel>(query.SearchTerm,skip,TagsPerPage);

            query.TotalPages= (int)Math.Ceiling(count / (decimal)PostPerPage);
            query.Tags = tags;

            return View(query);
        }
    }
}
