﻿using Ganss.XSS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using YourMoviesForum.Web.InputModels.Posts;

using static YourMoviesForum.Common.ErrorMessages.Post;
using static YourMoviesForum.Common.GlobalConstants;

namespace YourMoviesForum.Web.InputModels
{
    public class AddPostFormModel
    {
        [Required]
        [StringLength(
            Post.PostTitleMaxLength,
            ErrorMessage = TitleLengthErrorMessage, 
            MinimumLength =Post.PostTitleMinLength)]
        public string Title { get; init; }

        [Required]
        //[StringLength(
        //    PostContentMaxLength,
        //    ErrorMessage =ContentLengthErrorMessage,
        //    MinimumLength =PostContentMinLength)]
        [MinLength(Post.PostContentMinLength,ErrorMessage = ContentMinLengthErrorMessage)]
        [DataType(DataType.MultilineText)]
        public string Content { get; init; }

        public string SanitizedContent
           => new HtmlSanitizer().Sanitize(Content);

        [Required]
        [Display(Name =Category.CategoryDisplayName)]
        public int CategoryId { get; init; }

        [Required]
        [Display(Name = Tag.TagDisplayName)]
        public IEnumerable<int> TagIds { get; init; }

        public IEnumerable<PostCategoryViewModel> Categories { get; set; }
        public IEnumerable<PostTagViewModel> Tags { get; set; }
    }
}
