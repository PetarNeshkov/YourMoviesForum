using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static YourMoviesForum.Data.Common.DataValidation.Category;

namespace YourMoviesForum.Data.Models
{
    public class Category
    {
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
