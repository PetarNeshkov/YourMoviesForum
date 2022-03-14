using System;

namespace YourMoviesForum.Data.Common.Models
{
    public interface IAuditInfo
    {
        string CreatedOn { get; set; }

        string ModifiedOn { get; set; }
    }
}
