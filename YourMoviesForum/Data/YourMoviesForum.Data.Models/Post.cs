using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using YourMoviesForum.Data.Common.Models;
using static YourMoviesForum.Data.Common.DataValidation.Post;

namespace YourMoviesForum.Data.Models
{

    public class Post : BaseDeletetableModel<int>
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
        public string Content { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int Views { get; set; }

        public ICollection<Reply> Replies { get; set; }
        public ICollection<Tag> Tags { get; set; }
     
    }
}
