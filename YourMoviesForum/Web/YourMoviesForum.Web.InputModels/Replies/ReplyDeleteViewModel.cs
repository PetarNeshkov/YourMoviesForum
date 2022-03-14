using System;

using Ganss.XSS;

using YourMoviesForum.Web.InputModels.Reactions;

using static YourMoviesForum.Common.GlobalConstants;
namespace YourMoviesForum.Web.InputModels.Replies
{
    public class ReplyDeleteViewModel:ReactionsViewModel
    {
      private readonly IHtmlSanitizer sanitizer;
        public ReplyDeleteViewModel()
        {
            sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Add(IFrameTag);           
        }

        public int Id { get; init; }

        public string Content { get; init; }

        public string SanitizedContent
            => sanitizer.Sanitize(Content);

        public string CreatedOn { get; init; }

        public ReplyAuthorDetailsViewModel Author { get; set; }
    }
}
