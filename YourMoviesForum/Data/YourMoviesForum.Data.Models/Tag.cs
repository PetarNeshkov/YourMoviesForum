using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static YourMoviesForum.Data.Common.DataValidation.Tag;

namespace YourMoviesForum.Data.Models
{
    public class Tag
    {
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public ICollection<Post> Posts { get; init; } = new HashSet<Post>();
    }
}
