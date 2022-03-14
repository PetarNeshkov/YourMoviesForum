using System;

namespace YourMoviesForum.Data.Common.Models
{
    public abstract class BaseDeletetableModel<TKey> : BaseModel<TKey>, IDeletableEntity
    {
        public bool IsDeleted { get; set ; }
        public string DeletedOn { get ; set; }
    }
}
