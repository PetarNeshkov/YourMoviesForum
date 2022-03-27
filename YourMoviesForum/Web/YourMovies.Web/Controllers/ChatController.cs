using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using YourMovies.Web.Infrastructure;
using YourMoviesForum.Services.Data.Messages;
using YourMoviesForum.Services.Data.Users;
using YourMoviesForum.Web.InputModels.Chat;

namespace YourMovies.Web.Controllers
{
    [Authorize]
    public class ChatController : Controller
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
            var viewModel = new ChatMessageInputModel();
            var recievedMessages = await RecievedMessagesAndActivityAsync(viewModel);

            viewModel.Users = await userService.GetAllUsersAsync<ChatUserViewModel>();
            viewModel.RecievedMessages = recievedMessages;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Message(ChatMessageInputModel input)
        {
            if (!ModelState.IsValid)
            {
                input.Users = await userService.GetAllUsersAsync<ChatUserViewModel>();
                input.RecievedMessages=await RecievedMessagesAndActivityAsync(input);
                return View(input);
            }

            input.Users = await userService.GetAllUsersAsync<ChatUserViewModel>();
            input.RecievedMessages=await RecievedMessagesAndActivityAsync(input);

            await messageService.CreateMessageAsync(input.Message, User.Id(), input.ReceiverId);

            return View(input);
        }

        private async Task<IEnumerable<ChatConversationViewModel>> RecievedMessagesAndActivityAsync(ChatMessageInputModel input)
        {
            var recievedMessages = input.RecievedMessages = await messageService.GetAllMessagesAsync<ChatConversationViewModel>(User.Id());
            foreach (var user in recievedMessages)
            {
                user.LastMessage = await messageService.GetLastMessageAsync(User.Id(), user.Id);
                user.LastMessageActivity = await messageService.GetLastActivityAsync(User.Id(), user.Id);
            }

            return recievedMessages;
        }
    }
}
