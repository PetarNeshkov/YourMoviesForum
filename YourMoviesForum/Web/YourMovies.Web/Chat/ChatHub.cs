using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

using YourMoviesForum.Services.Data.Messages;
using YourMoviesForum.Services.Data.Users;
using YourMoviesForum.Services.Providers.DateTime;
using YourMoviesForum.Web.InputModels.Chat;

namespace YourMovies.Web.Chat
{
    public class ChatHub:Hub
    {
        private readonly IUserService userService;
        private readonly IMessageService messagesService;
        private readonly IDateTimeProvider dateTimeProvider;

        public ChatHub(
            IUserService userService,
            IMessageService messagesService, 
            IDateTimeProvider dateTimeProvider)
        {
            this.userService = userService;
            this.messagesService = messagesService;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task SendMessageToUser(string message, string receiverId)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var authorId = Context.User.Id();
            var user = await userService.GetUserByIdAsync<ChatUserViewModel>(authorId);

            await messagesService.CreateMessageAsync(message, authorId, receiverId);
            await Clients.All.SendAsync(
                "ReceiveMessageFromTheOtherUser",
                new ChatConversationWithUserInputModel
                {
                    AuthorId = authorId,
                    AuthorUserName = user.UserName,
                    Content = message,
                    FirstLetter= await userService.GetUserFirstLetterAsync(user.Id),
                    BackgroundColor=await userService.GetUserBackGroundColorAsync(user.Id),
                    CreatedOn = dateTimeProvider.Now()
                });
        }
        public async Task Typing(string name)
            => await Clients.Others.SendAsync("CurrentlyTyping", name);

    }
}
