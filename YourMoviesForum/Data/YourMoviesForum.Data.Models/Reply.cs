using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static YourMoviesForum.Data.Common.DataValidation.Reply;

namespace YourMoviesForum.Data.Models
{
    public class Reply
    {
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(ContentMaxLength)]
        [Column("Content")]
        public string Content { get; set; }

        public DateTime RepliedOn { get; set; }

        public bool IsDeleted { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }

    }
}
