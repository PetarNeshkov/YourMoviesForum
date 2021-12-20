using System.Collections.Generic;
using YourMovies.Web.Views.Pagination;
using YourMoviesForum.Web.InputModels.Home;
using YourMoviesForum.Web.InputModels.Posts;

namespace YourMoviesForum.Web.InputModels.Tags
{
    public class TagDetailsViewModel
    {
        public int Id { get; init; }
        public TagsListingViewModel Tag { get; init; }
        public IEnumerable<PostListingViewModel> Posts { get; init; }

        public PaginationViewModel Pagination { get; set; }
    }
}
