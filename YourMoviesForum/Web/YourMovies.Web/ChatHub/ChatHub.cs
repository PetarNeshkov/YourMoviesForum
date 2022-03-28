﻿using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

using YourMovies.Web.Infrastructure;
using YourMoviesForum.Services.Data.Messages;
using YourMoviesForum.Services.Data.Users;
using YourMoviesForum.Services.Providers.DateTime;
using YourMoviesForum.Web.InputModels.Chat;

namespace YourMovies.Web.ChatHub
{
    public class ChatHub:Hub
    {
        private readonly IUserService usersService;
        private readonly IMessageService messagesService;
        private readonly IDateTimeProvider dateTimeProvider;

        public ChatHub(
            IUserService usersService,
            IMessageService messagesService, 
            IDateTimeProvider dateTimeProvider)
        {
            this.usersService = usersService;
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
            var user = await usersService.GetUserByIdAsync<ChatUserViewModel>(authorId);

            await this.messagesService.CreateMessageAsync(message, authorId, receiverId);
            await this.Clients.All.SendAsync(
                "ReceiveMessageFromTheOtherUser",
                new ChatConversationWithUserViewModel
                {
                    AuthorId = authorId,
                    AuthorUserName = user.UserName,
                    Content = message,
                    CreatedOn = dateTimeProvider.Now()
                });
        }
    }
}