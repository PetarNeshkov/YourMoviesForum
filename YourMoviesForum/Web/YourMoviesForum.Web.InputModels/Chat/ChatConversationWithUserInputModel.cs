
using Ganss.XSS;

namespace YourMoviesForum.Web.InputModels.Chat
{
    public class ChatConversationWithUserInputModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public ChatConversationWithUserInputModel()
        {
            sanitizer = new HtmlSanitizer();
        }

        public string Content { get; init; }

        public string SanitizedContent
            => sanitizer.Sanitize(Content);

        public string AuthorId { get; init; }

        public string AuthorUserName { get; init; }

        public string CreatedOn { get; init; }
    }
}
