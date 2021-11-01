using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static YourMoviesForum.Data.Common.DataValidation.Post;

namespace YourMoviesForum.Data.Models
{

    public class Post
    {
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        [Column("Column")]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Views { get; set; }

        public ICollection<Reply> Replies { get; init; } = new HashSet<Reply>();
        public ICollection<Tag> Tags { get; init; } = new HashSet<Tag>();
    }
}
