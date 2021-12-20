using System.Collections.Generic;
using YourMovies.Web.Views.Pagination;

namespace YourMoviesForum.Web.InputModels.Tags
{
    public class AllTagsQueryModel
    {
        public int PostsCount { get; init; }
        public string SearchTerm { get; init; }

        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<TagsListingViewModel> Tags { get; set; }
    }
}
