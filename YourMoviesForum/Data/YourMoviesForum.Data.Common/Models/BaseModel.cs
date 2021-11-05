using System;
using System.ComponentModel.DataAnnotations;

namespace YourMoviesForum.Data.Common.Models
{
    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Key]
        public TKey Id { get; init; }
        public  DateTime CreatedOn { get; set; }
        public  DateTime? ModifiedOn { get; set; }
    }
}
