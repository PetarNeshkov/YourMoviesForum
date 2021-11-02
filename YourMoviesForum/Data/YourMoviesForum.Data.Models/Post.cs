using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using YourMoviesForum.Data.Common.Models;
using static YourMoviesForum.Data.Common.DataValidation.Post;

namespace YourMoviesForum.Data.Models
{

    public class Post : BaseDeletetableModel<string>
    {
        public Post()
        {
            Replies = new HashSet<Reply>();
            Tags = new HashSet<Tag>();
        }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        [Column("Column")]
        public string Content { get; set; }

        public int Views { get; set; }

        public ICollection<Reply> Replies { get; init; }
        public ICollection<Tag> Tags { get; init; }
     
    }
}
