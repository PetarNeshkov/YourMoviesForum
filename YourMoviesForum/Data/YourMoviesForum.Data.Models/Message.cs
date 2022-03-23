using System.ComponentModel.DataAnnotations;

using YourMoviesForum.Data.Common.Models;

using static YourMoviesForum.Data.Common.DataValidation.Message;

namespace YourMoviesForum.Data.Models
{
    public class Message:BaseModel<int>
    {
        [Required]
        [StringLength(ContentMaxLength)]
        public string Content { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public string ReceiverId { get; set; }

        public ApplicationUser Receiver { get; set; }
    }
}
