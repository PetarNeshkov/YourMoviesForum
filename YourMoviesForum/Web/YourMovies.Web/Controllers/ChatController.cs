using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YourMovies.Web.Infrastructure;
using YourMoviesForum.Services.Data.Messages;
using YourMoviesForum.Services.Data.Users;
using YourMoviesForum.Web.InputModels.Chat;

namespace YourMovies.Web.Controllers
{
    public class ChatController:Controller
    {
        private readonly IUserService userService;
        private readonly IMessageService messageService;

        public ChatController(IUserService userService, IMessageService messageService)
        {
            this.userService = userService;
            this.messageService = messageService;
        }

        public async Task<IActionResult> Message()
        {
            var recievedMessages = await messageService.GetAllMessagesAsync<ChatConversationViewModel>(User.Id());

            foreach (var user in recievedMessages)
            {
                user.LastMessage = await messageService.GetLastMessageAsync(User.Id(), user.Id);
                user.LastMessageActivity = await messageService.GetLastActivityAsync(User.Id(), user.Id);
            }

            var viewModel = new ChatMessageInputModel
            {
                Users = await this.userService.GetAllUsersAsync<ChatUserViewModel>(),
                RecievedMessages= recievedMessages
            };

            return this.View(viewModel);
        }
    }
}
