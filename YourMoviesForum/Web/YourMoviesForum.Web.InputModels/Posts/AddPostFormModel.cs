using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using YourMoviesForum.Web.InputModels.Posts;
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
        [DataType(DataType.MultilineText)]
        public string Content { get; init; }

        [Required]
        [Url]
        [Display(Name ="Image Url")]
        public string ImageUrl { get; init; }

        [Required]
        [Display(Name ="Categories")]
        public int CategoryId { get; init; }

        [Required]
        [Display(Name ="Tags")]
        public IEnumerable<int> TagIds { get; init; }

        public IEnumerable<PostCategoryViewModel> Categories { get; set; }
        public IEnumerable<PostsTagViewModel> Tags { get; set; }
    }
}
