using System.Collections.Generic;

namespace YourMoviesForum.Web.InputModels.Home
{
    public class IndexViewModel
    {
        public int TotalPosts { get; init; }

        public int TotalUsers { get; init; }

        public IEnumerable<PostListingViewModel> Posts { get; init; }
    }
}
