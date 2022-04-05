using System;

using Ganss.XSS;

using YourMoviesForum.Web.InputModels.Reactions;

using static YourMoviesForum.Common.GlobalConstants;

namespace YourMoviesForum.Web.InputModels.Posts
{
    public class PostRepliesDetailsViewModel:ReactionsViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public PostRepliesDetailsViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add(IFrameTag);
        }

        public int Id { get; init; }

        public string Content { get; init; }

        public string SanitizedContent
            => this.sanitizer.Sanitize(Content);

        public char FirstLetter { get; set; }
        public string BackgroundColor { get; set; }

        public int Likes { get; init; }

        public string CreatedOn { get; init; }

        public int? ParentId { get; init; }

        public PostAuthorDetailsViewModel Author { get; init; }
    }
}
