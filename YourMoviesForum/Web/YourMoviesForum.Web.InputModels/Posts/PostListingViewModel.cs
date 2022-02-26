using System.Collections.Generic;

using YourMoviesForum.Web.InputModels.Posts;

namespace YourMoviesForum.Web.InputModels.Home
{
    public class PostListingViewModel
    {
        public int Id { get; init; }

        public string Title { get; init; }
        public bool IsDeleted { get; init; }

        public int Rating { get; init; }

        public PostCategoryViewModel Category { get; init; }
        public  IEnumerable<PostTagViewModel> Tags { get; set; }
    }
}
