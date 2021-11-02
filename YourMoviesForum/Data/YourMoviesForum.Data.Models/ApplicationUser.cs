using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

using YourMoviesForum.Data.Common.Models;
using static YourMoviesForum.Data.Common.DataValidation.User;

namespace YourMoviesForum.Data.Models
{
    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            Posts = new HashSet<Post>();
            Replies = new HashSet<Reply>();
            //Roles = new HashSet<IdentityUserRole<string>>();
        }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        //Audit Info
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        //Deletable entity
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }

        public ICollection<Post> Posts { get; init; }
        public ICollection<Reply> Replies { get; init; }

        //public virtual ICollection<IdentityUserRole<string>> Roles { get; init; }
    }
}
