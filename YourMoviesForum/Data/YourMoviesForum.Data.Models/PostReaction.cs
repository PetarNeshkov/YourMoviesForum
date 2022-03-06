using YourMoviesForum.Data.Common.Models;
using YourMoviesForum.Web.InputModels.Reactions.enums;

namespace YourMoviesForum.Data.Models
{
    public class PostReaction:BaseModel<int>
    {
        public ReactionType ReactionType { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }
    }
}
