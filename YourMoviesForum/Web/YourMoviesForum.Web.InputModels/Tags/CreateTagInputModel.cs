using System.ComponentModel.DataAnnotations;

using static YourMoviesForum.Common.ErrorMessages.Tags;
using static YourMoviesForum.Common.GlobalConstants.Tags;

namespace YourMoviesForum.Web.InputModels.Tags
{
    public class CreateTagInputModel
    {
        [Required]
        [StringLength(TagNameMaxLength, ErrorMessage = TagNameLengthErrorMessage, MinimumLength = TagNameMinLength)]
        public string Name { get; set; }
    }
}
