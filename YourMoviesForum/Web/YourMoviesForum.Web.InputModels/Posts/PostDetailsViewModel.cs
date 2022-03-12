using System;
using System.Collections.Generic;

using Ganss.XSS;
using YourMoviesForum.Common;
using YourMoviesForum.Web.InputModels.Reactions;

namespace YourMoviesForum.Web.InputModels.Posts
{
    public class PostDetailsViewModel:ReactionsViewModel
    {
        private readonly IHtmlSanitizer sanitizer;
        public PostDetailsViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add(GlobalConstants.IFrameTag);
        }

        public int Id { get; init; }
        public string Title { get; init; }
        public DateTime CreatedOn { get; init; }
        public string Content { get; init; }

        public string SanitizedContent
            => this.sanitizer.Sanitize(Content);

        public PostAuthorDetailsViewModel Author { get; init; }

        public PostCategoryViewModel Category { get; init; }

        public IEnumerable<PostTagViewModel> Tags { get; set; }

        public IEnumerable<PostRepliesDetailsViewModel> Replies { get; set; }
    }
}
