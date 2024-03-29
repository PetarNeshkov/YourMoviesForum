﻿using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using YourMoviesForum.Data.Common.Models;
using static YourMoviesForum.Data.Common.DataValidation.Post;

namespace YourMoviesForum.Data.Models
{

    public class Post : BaseDeletetableModel<int>
    {
        public Post()
        {
            Replies = new HashSet<Reply>();
            Tags = new HashSet<Tag>();
            Reactions = new HashSet<PostReaction>(); 
        }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int Rating { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public ICollection<Reply> Replies { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<PostReaction> Reactions { get; set; }

    }
}
