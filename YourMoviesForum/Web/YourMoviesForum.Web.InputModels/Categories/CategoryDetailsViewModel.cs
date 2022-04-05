using System.Collections.Generic;

using YourMovies.Web.Views.Pagination;
using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.User;

namespace YourMoviesForum.Web.InputModels.Categories
{
    public class CategoryDetailsViewModel:UserBannerViewModel
    {
        public int Id { get; init; }
        public CategoryListingViewModel Category { get; init; }
        public IEnumerable<PostListingViewModel> Posts { get; init; }

        public PaginationViewModel Pagination { get; set; }
    }
}
