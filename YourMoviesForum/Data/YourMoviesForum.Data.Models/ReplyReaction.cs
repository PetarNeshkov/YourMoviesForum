using YourMoviesForum.Data.Common.Models;
using YourMoviesForum.Web.InputModels.Reactions.enums;

namespace YourMoviesForum.Data.Models
{
    public class ReplyReaction:BaseModel<int>
    {
        public ReactionType ReactionType { get; set; }

        public int ReplyId { get; set; }

        public Reply Reply { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }
    }
}
