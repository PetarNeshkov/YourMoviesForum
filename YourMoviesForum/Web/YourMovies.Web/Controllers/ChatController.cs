using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

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
            var viewModel = new ChatMessageInputModel()
            {
                Users = await userService.GetAllUsersAsync<ChatUserViewModel>(),
                RecievedMessages = await RecievedMessagesAndActivityAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Message(ChatMessageInputModel input)
        {
            if (!ModelState.IsValid)
            {
                input.Users = await userService.GetAllUsersAsync<ChatUserViewModel>();
                input.RecievedMessages=await RecievedMessagesAndActivityAsync();
                return View(input);
            }

            await messageService.CreateMessageAsync(input.Message, User.Id(), input.ReceiverId);

            return RedirectToAction(nameof(PrivateChat),new { id = input.ReceiverId });
        }

        public async Task<IActionResult> PrivateChat(string id)
        {
            var messagesWithCurrentUser = await messageService.GetAllUserMessagesAsync<ChatConversationWithUserInputModel>(User.Id(), id);

            foreach (var message in messagesWithCurrentUser)
            {
                message.FirstLetter= await userService.GetUserFirstLetterAsync(message.AuthorId);
                message.BackgroundColor=await userService.GetUserBackGroundColorAsync(message.AuthorId);
            }

            var viewModel = new ChatWithUserViewModel
            {
                User = await userService.GetUserByIdAsync<ChatUserViewModel>(id),
                MessagesWithCurrentUser =messagesWithCurrentUser,
                RecievedMessages = await RecievedMessagesAndActivityAsync()
            };

            return View(viewModel);
        }

        private async Task<IEnumerable<ChatConversationViewModel>> RecievedMessagesAndActivityAsync()
        {
            var recievedMessages = await messageService.GetAllMessagesAsync<ChatConversationViewModel>(User.Id());
            foreach (var user in recievedMessages)
            {
                user.FirstLetter=await userService.GetUserFirstLetterAsync(user.Id);
                user.BackgroundColor=await userService.GetUserBackGroundColorAsync(user.Id);
                user.LastMessage = await messageService.GetLastMessageAsync(User.Id(), user.Id);
                user.LastMessageActivity = await messageService.GetLastActivityAsync(User.Id(), user.Id);
            }

            return recievedMessages;
        }
    }
}
