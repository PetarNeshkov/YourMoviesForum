using Ganss.XSS;
using System.ComponentModel.DataAnnotations;

using static YourMoviesForum.Common.GlobalConstants;
using static YourMoviesForum.Common.GlobalConstants.Reply;

namespace YourMoviesForum.Web.InputModels.Replies
{
    public class EditReplyFormModel
    {
        private readonly IHtmlSanitizer sanitizer;
        public EditReplyFormModel()
        {
            sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Add(IFrameTag);
        }
        public int Id { get; init; }

        public int PostId { get; init; }

        [Required]
        [MaxLength(ReplyContentMaxLength)]
        public string Content { get; init; }

        public string SanitizedContent
           => sanitizer.Sanitize(Content);

        public string AuthorId { get; init; }
    }
}
