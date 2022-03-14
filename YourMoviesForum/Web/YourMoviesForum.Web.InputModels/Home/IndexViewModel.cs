using System.Collections.Generic;

namespace YourMoviesForum.Web.InputModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<PostListingViewModel> Posts { get; init; }
    }
}
