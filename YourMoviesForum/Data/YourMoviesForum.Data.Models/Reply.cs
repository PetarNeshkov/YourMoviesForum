using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using YourMoviesForum.Data.Common.Models;

using static YourMoviesForum.Data.Common.DataValidation.Reply;

namespace YourMoviesForum.Data.Models
{
    public class Reply:BaseDeletetableModel<int>
    {
        public Reply()
        {
            Reactions = new HashSet<ReplyReaction>();
        }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public int? ParentId { get; set; }

        public Reply Parent { get; set; }

        public ICollection<ReplyReaction> Reactions { get; set; }
    }
}
