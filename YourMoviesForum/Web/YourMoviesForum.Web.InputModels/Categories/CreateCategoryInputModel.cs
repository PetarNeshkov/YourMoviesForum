using System.ComponentModel.DataAnnotations;

using static YourMoviesForum.Common.ErrorMessages.Categories;
using static YourMoviesForum.Common.GlobalConstants.Category;

namespace YourMoviesForum.Web.InputModels.Categories
{
    public class CreateCategoryInputModel
    {
        [Required]
        [StringLength(CategoryNameMaxLength, ErrorMessage = CategoryNameLengthErrorMessage, MinimumLength = CategoryNameMinLength)]
        public string Name { get; set; }
    }
}
