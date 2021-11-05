using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using YourMoviesForum.Data.Common.Models;
using static YourMoviesForum.Data.Common.DataValidation.Tag;

namespace YourMoviesForum.Data.Models
{
    public class Tag:BaseDeletetableModel<int>
    {
        public Tag()
        {
            Posts = new HashSet<Post>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public ICollection<Post> Posts { get; init; }
    }
}
