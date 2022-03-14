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
        public string CreatedOn { get ; set; }
        public string ModifiedOn { get; set ; }

        //Deletable Entity
        public bool IsDeleted { get; set; }
        public string DeletedOn { get; set; }
    }
}
