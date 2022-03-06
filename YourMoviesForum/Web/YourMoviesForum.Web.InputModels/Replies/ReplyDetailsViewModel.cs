﻿using Ganss.XSS;
using System;
using System.Collections.Generic;
using YourMoviesForum.Web.InputModels.Posts;

using static YourMoviesForum.Common.GlobalConstants;

namespace YourMoviesForum.Web.InputModels.Replies
{
    public class ReplyDetailsViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public ReplyDetailsViewModel()
        {
            sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Add(IFrameTag);
        }

        public int Id { get; init; }

        public string Content { get; init; }

        public string SanitizedContent
            => sanitizer.Sanitize(Content);
        public DateTime CreatedOn { get; init; }

        public int PostId { get; init; }

        public string PostAuthorId { get; init; }

        public int? ParentId { get; init; }

        public PostAuthorDetailsViewModel Author { get; init; }

        public IEnumerable<ReplyDetailsViewModel> Replies { get; set; }
    }
}