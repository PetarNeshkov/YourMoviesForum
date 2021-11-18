using System.Collections.Generic;

using YourMoviesForum.Web.InputModels.Posts;
using YourMoviesForum.Web.InputModels.Tags;

namespace YourMoviesForum.Web.InputModels.Home
{
    public class PostListingViewModel
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public PostCategoryViewModel Category { get; init; }
        public  IEnumerable<PostsTagViewModel> Tags { get; init; }
    }
}
