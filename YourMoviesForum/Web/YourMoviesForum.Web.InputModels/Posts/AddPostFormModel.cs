using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using YourMoviesForum.Web.InputModels.Tags;

using static YourMoviesForum.Common.ErrorMessages;
using static YourMoviesForum.Common.GlobalConstants;

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
        [StringLength(
            PostContentMaxLength,
            ErrorMessage =ContentLengthErrorMessage,
            MinimumLength =PostContentMinLength)]
        public string Content { get; init; }

        [Required]
        [Url]
        [Display(Name ="Image")]
        public string ImageUrl { get; init; }

        [Required]
        [Display(Name ="Category")]
        public string CategoryId { get; init; }
        public IEnumerable<PostsTagViewModel> Tags { get; set; }
    }
}
