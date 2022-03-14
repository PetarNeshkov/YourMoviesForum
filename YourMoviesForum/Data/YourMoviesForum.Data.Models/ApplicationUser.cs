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
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }

        //Deletable entity
        public bool IsDeleted { get; set; }
        public string DeletedOn { get; set; }

        public ICollection<Post> Posts { get; init; }
        public ICollection<Reply> Replies { get; init; }

        public  ICollection<IdentityUserRole<string>> Roles { get; init; }

        public  ICollection<IdentityUserClaim<string>> Claims { get; init; }

        public  ICollection<IdentityUserLogin<string>> Logins { get; init; }
    }
}
