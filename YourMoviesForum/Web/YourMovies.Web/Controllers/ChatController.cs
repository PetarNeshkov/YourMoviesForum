using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> Message(ChatMessageInputModel query)
        {
            query.Users = await userService.GetAllUsersAsync<ChatUserViewModel>();

            return View(query);
        }
    }
}
