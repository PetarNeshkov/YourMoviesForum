using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using YourMoviesForum.Data.Common.Models;
using static YourMoviesForum.Data.Common.DataValidation.Category;

namespace YourMoviesForum.Data.Models
{
    public class Category:BaseDeletetableModel<string>
    {
        public Category()
        {
            Posts = new HashSet<Post>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
        public ICollection<Post> Posts { get; set;}
    }
}
