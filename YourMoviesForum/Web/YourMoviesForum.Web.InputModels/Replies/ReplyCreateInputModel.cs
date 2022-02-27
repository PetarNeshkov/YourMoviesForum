using System.ComponentModel.DataAnnotations;

using static YourMoviesForum.Common.GlobalConstants.Reply;

namespace YourMoviesForum.Web.InputModels.Replies
{
    public class ReplyCreateInputModel
    {
        public int? ParentId { get; init; }

        [Required]
        public int PostId { get; init; }

        [Required]
        [MaxLength(ReplyContentMaxLength)]
        public string Content { get; init; }
    }
}
