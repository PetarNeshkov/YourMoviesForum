

using System.Collections.Generic;

namespace YourMoviesForum.Web.InputModels.Chat
{
    public class ChatWithUserViewModel
    {
        public ChatUserViewModel User { get; init; }

        public IEnumerable<ChatConversationWithUserInputModel> Messages { get; init; }
    }
}
