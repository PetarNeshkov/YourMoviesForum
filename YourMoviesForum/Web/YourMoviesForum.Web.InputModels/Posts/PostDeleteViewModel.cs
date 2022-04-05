using Ganss.XSS;
using System;
using System.Collections.Generic;

using YourMoviesForum.Common;
using YourMoviesForum.Web.InputModels.Reactions;

namespace YourMoviesForum.Web.InputModels.Posts
{
    public class PostDeleteViewModel: ReactionsViewModel
    {
        private readonly IHtmlSanitizer sanitizer;
        public PostDeleteViewModel()
        {
            sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Add(GlobalConstants.IFrameTag);
        }
        public int Id { get; init; }
        public string Title { get; init; }
        public string CreatedOn { get; init; }
        public char FirstLetter { get; set; }
        public string BackgroundColor { get; set; }
        public string Content { get; init; }

        public string SanitizedContent
         => this.sanitizer.Sanitize(Content);

        public PostAuthorDetailsViewModel Author { get; init; }

        public PostCategoryViewModel Category { get; init; }

        public IEnumerable<PostTagViewModel> Tags { get; set; }
    }
}
