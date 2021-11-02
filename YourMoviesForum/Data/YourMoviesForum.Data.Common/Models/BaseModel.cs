using System;
using System.ComponentModel.DataAnnotations;

using static YourMoviesForum.Data.Common.DataValidation;

namespace YourMoviesForum.Data.Common.Models
{
    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Key]
        [Required]
        [MaxLength(IdMaxLength)]
        public TKey Id { get; init; }
        public  DateTime CreatedOn { get; set; }
        public  DateTime? ModifiedOn { get; set; }
    }
}
