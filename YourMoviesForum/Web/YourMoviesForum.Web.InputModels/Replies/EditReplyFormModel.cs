using System.ComponentModel.DataAnnotations;

using static YourMoviesForum.Common.GlobalConstants.Reply;

namespace YourMoviesForum.Web.InputModels.Replies
{
    public class EditReplyFormModel
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(ReplyContentMaxLength)]
        public string Content { get; init; }

        public string AuthorId { get; init; }
    }
}
