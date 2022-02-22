using System.Collections.Generic;

using YourMovies.Web.Views.Pagination;
using YourMoviesForum.Web.InputModels.Categories;

namespace YourMovies.Web.Views.Categories
{
    public class AllCategoriesQueryModel
    {
        public int PostsCount { get; init; }
        public string SearchTerm { get; init; }

        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<CategoryListingViewModel> Categories { get; set; }
    }
}
