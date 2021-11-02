using System;

using Microsoft.AspNetCore.Identity;

using YourMoviesForum.Data.Common.Models;


namespace YourMoviesForum.Data.Models
{
    public class ApplicationRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public ApplicationRole()
                :this(null)
        {

        }

        public ApplicationRole(string name)
                :base(name)
        {
            Id = Guid.NewGuid().ToString();
        }

        //Audit Info
        public DateTime CreatedOn { get ; set; }
        public DateTime? ModifiedOn { get; set ; }

        //Deletable Entity
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
