using System.Collections.Generic;
using YourMoviesForum.Web.InputModels.Posts;

namespace YourMoviesForum.Web.InputModels.Replies
{
    public class NestedReplies
    {
        public int? ParentId { get; init; }

        public IEnumerable<PostRepliesDetailsViewModel> Replies { get; init; }
    }
}
