
using Ganss.XSS;

namespace YourMoviesForum.Web.InputModels.Chat
{
    public class ChatConversationWithUserViewModel
    {
        private readonly IHtmlSanitizer sanitizer;

        public ChatConversationWithUserViewModel()
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
