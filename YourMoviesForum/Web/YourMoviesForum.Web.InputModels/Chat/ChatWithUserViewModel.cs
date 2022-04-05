using System.Collections.Generic;
using YourMoviesForum.Web.InputModels.User;

namespace YourMoviesForum.Web.InputModels.Chat
{
    public class ChatWithUserViewModel:UserBannerViewModel
    {
        public ChatUserViewModel User { get; init; }

        public IEnumerable<ChatConversationWithUserInputModel> MessagesWithCurrentUser { get; init; }

        public IEnumerable<ChatConversationViewModel> RecievedMessages { get; set; }

    }
}
