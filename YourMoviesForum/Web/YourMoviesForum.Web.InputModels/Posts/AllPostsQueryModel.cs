using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using YourMovies.Web.Views.Pagination;
using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.User;

namespace YourMoviesForum.Web.InputModels.Posts
{
    public class AllPostsQueryModel:UserBannerViewModel
    {
        [Display(Name = "Search Bar")]
        public string SearchTerm { get; init; }

        public PostSorting Sorting { get; init; }

        public PaginationViewModel Pagination { get; set; }
        public IEnumerable<PostListingViewModel> Posts { get; set; }
    }
}

