
using Ganss.XSS;
using YourMoviesForum.Web.InputModels.User;

namespace YourMoviesForum.Web.InputModels.Chat
{
    public class ChatConversationWithUserInputModel:UserBannerViewModel
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
