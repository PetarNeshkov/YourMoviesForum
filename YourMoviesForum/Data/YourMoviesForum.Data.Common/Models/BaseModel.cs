using System.ComponentModel.DataAnnotations;

namespace YourMoviesForum.Data.Common.Models
{
    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Key]
        public TKey Id { get; init; }
        public  string CreatedOn { get; set; }
        public  string ModifiedOn { get; set; }
    }
}
