using Ganss.XSS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using YourMoviesForum.Web.InputModels.Posts;
using YourMoviesForum.Web.InputModels.Tags;

using static YourMoviesForum.Common.ErrorMessages.Post;
using static YourMoviesForum.Common.GlobalConstants.Post;

namespace YourMoviesForum.Web.InputModels
{
    public class AddPostFormModel
    {
        [Required]
        [StringLength(
            PostTitleMaxLength,
            ErrorMessage = TitleLengthErrorMessage, 
            MinimumLength =PostTitleMinLength)]
        public string Title { get; init; }

        [Required]
        //[StringLength(
        //    PostContentMaxLength,
        //    ErrorMessage =ContentLengthErrorMessage,
        //    MinimumLength =PostContentMinLength)]
        [MinLength(PostContentMinLength,ErrorMessage = ContentMinLengthErrorMessage)]
        [DataType(DataType.MultilineText)]
        public string Content { get; init; }

        public string SanitizedContent
           => new HtmlSanitizer().Sanitize(Content);

        [Required]
        [Display(Name ="Categories")]
        public int CategoryId { get; init; }

        [Required]
        [Display(Name ="Tags")]
        public IEnumerable<int> TagIds { get; init; }

        public IEnumerable<PostCategoryViewModel> Categories { get; set; }
        public IEnumerable<PostTagViewModel> Tags { get; set; }
    }
}
