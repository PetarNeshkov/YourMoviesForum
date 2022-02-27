using Ganss.XSS;
using System;
using static YourMoviesForum.Common.GlobalConstants;

namespace YourMoviesForum.Web.InputModels.Posts
{
    public class PostRepliesDetailsViewModel
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

        public int Likes { get; init; }

        public DateTime CreatedOn { get; init; }

        public int? ParentId { get; init; }

        public PostAuthorDetailsViewModel Author { get; init; }
    }
}
