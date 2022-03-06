﻿using System;

using Ganss.XSS;

using static YourMoviesForum.Common.GlobalConstants;
namespace YourMoviesForum.Web.InputModels.Replies
{
    public class ReplyDeleteViewModel
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

        public DateTime CreatedOn { get; init; }

        public ReplyAuthorDetailsViewModel Author { get; set; }
    }
}