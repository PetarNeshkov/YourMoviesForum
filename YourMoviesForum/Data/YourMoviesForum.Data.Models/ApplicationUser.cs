using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

using YourMoviesForum.Data.Common.Models;

namespace YourMoviesForum.Data.Models
{
    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            Posts = new HashSet<Post>();
            Replies = new HashSet<Reply>();

            Roles = new HashSet<IdentityUserRole<string>>();
            Claims = new HashSet<IdentityUserClaim<string>>();
            Logins = new HashSet<IdentityUserLogin<string>>();
        }

        public int Rating { get; set; }

        //Audit Info
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        //Deletable entity
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }

        public ICollection<Post> Posts { get; init; }
        public ICollection<Reply> Replies { get; init; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; init; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; init; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; init; }
    }
}
