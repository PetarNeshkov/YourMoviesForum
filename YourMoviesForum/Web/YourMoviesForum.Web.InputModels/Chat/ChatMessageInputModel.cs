using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static YourMoviesForum.Common.GlobalConstants.Message;

namespace YourMoviesForum.Web.InputModels.Chat
{
    public class ChatMessageInputModel
    {
        [Required]
        [MaxLength(MessageMaxLength)]
        public string Message { get; init; }

        [Required]
        public string ReceiverId { get; init; }

        public IEnumerable<ChatUserViewModel> Users { get; set; }

        public IEnumerable<ChatConversationViewModel> RecievedMessages {get;set;}
    }
}
