using System.Collections.Generic;
using YourMovies.Web.Views.Pagination;
using YourMoviesForum.Web.InputModels.Home;

namespace YourMoviesForum.Web.InputModels.Categories
{
    public class CategoryDetailsViewModel
    {
        public int Id { get; init; }
        public CategoryListingViewModel Category { get; init; }
        public IEnumerable<PostListingViewModel> Posts { get; init; }

        public PaginationViewModel Pagination { get; set; }
    }
}
