﻿using Ganss.XSS;
using System;
using System.Collections.Generic;

using YourMoviesForum.Common;

namespace YourMoviesForum.Web.InputModels.Posts
{
    public class PostDeleteViewModel
    {
        private readonly IHtmlSanitizer sanitizer;
        public PostDeleteViewModel()
        {
            sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Add(GlobalConstants.IFrameTag);
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
    }
}