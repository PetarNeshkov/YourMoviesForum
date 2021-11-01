using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static YourMoviesForum.Data.Common.DataValidation.User;

namespace YourMoviesForum.Data.Models
{
    public class User
    {
        [Required]
        [MaxLength(IdMaxLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Post> Posts { get; init; } = new HashSet<Post>();
        public ICollection<Reply> Replies { get; init; } = new HashSet<Reply>();
    }
}
