using System.ComponentModel.DataAnnotations;

using Ganss.XSS;

using static YourMoviesForum.Common.GlobalConstants;
using static YourMoviesForum.Common.ErrorMessages.Post;
using System.Collections.Generic;

namespace YourMoviesForum.Web.InputModels.Posts
{
    public class EditPostFormModel
    {
        public int Id { get; init; }

        [Required]
        [StringLength(
            Post.PostTitleMaxLength,
            ErrorMessage = TitleLengthErrorMessage,
            MinimumLength = Post.PostTitleMinLength)]
        public string Title { get; init; }

        [Required]
        //[StringLength(
        //    PostContentMaxLength,
        //    ErrorMessage =ContentLengthErrorMessage,
        //    MinimumLength =PostContentMinLength)]
        [MinLength(Post.PostContentMinLength, ErrorMessage = ContentMinLengthErrorMessage)]
        [DataType(DataType.MultilineText)]
        public string Content { get; init; }

        public string SanitizedContent
          => new HtmlSanitizer().Sanitize(Content);

        [Required]
        public int CategoryId { get; init; }

        [Required]
        [Display(Name =Tag.TagDisplayName)]
        public ICollection<int> TagIds { get; init; }

        public string AuthorId { get; init; }

        public IEnumerable<PostTagViewModel> Tags { get; set; }

        public IEnumerable<PostCategoryViewModel> Categories { get; set; }
    }
}
