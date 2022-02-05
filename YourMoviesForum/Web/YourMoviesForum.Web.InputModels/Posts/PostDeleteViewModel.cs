using Ganss.XSS;
using System;
using System.Collections.Generic;

namespace YourMoviesForum.Web.InputModels.Posts
{
    public class PostDeleteViewModel
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public DateTime CreatedOn { get; init; }
        public string Content { get; init; }

        public string SanitizedContent
            => new HtmlSanitizer().Sanitize(Content);

        public PostAuthorDetailsViewModel Author { get; init; }

        public PostCategoryViewModel Category { get; init; }

        public IEnumerable<PostTagViewModel> Tags { get; set; }
    }
}
