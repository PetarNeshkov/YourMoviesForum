

namespace YourMoviesForum.Web.InputModels.Chat
{
    public class ChatConversationViewModel
    {
        public string Id { get; init; }

        public string UserName { get; init; }

        public string LastMessage { get; set; }

        public string LastMessageActivity { get; set; }
    }
}
