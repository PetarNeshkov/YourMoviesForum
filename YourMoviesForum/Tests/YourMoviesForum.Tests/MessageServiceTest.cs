using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using FluentAssertions;
using Xunit;
using YourMoviesForum.Services.Data.Messages;
using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Providers.DateTime;
using Moq;
using System.Collections.Generic;
using AutoMapper;

namespace YourMoviesForum.Tests
{
    public class MessageServiceTest
    {
        [Fact]
        public async Task CreateMessageAsyncShouldAddMessageInDatabase()
        {
            var guid = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<YourMoviesDbContext>()
                .UseInMemoryDatabase(guid)
                .Options;

            var db = new YourMoviesDbContext(options);

            var messagesService = new MessageService(db, null);
            await messagesService.CreateMessageAsync("Test", "1", "2");

            db.Messages.Should().HaveCount(1);
        }

        [Fact]
        public async Task CreateMessageAsyncShouldAddRightMessageInDatabase()
        {
            var guid = Guid.NewGuid().ToString();
            var options = DatabaseConfigOptions(guid);

            var db = new YourMoviesDbContext(options);

            var messagesService = new MessageService(db, null);
            await messagesService.CreateMessageAsync("Test", "1", "3");

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now())
                    .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            var expected = new Message
            {
                Id = 1,
                Content = "Test",
                AuthorId = "1",
                ReceiverId = "3",
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var actual = await db.Messages.FirstOrDefaultAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetLastMessagesAsyncShouldReturnProperValue()
        {
            var guid = Guid.NewGuid().ToString();
            var options = DatabaseConfigOptions(guid);

            var db = new YourMoviesDbContext(options);

            var messages = new List<Message>
            {
                new Message
                {
                    Content = "Test",
                    AuthorId="1",
                    ReceiverId="3",
                },
                new Message
                {
                    Content="Test-Hello",
                    AuthorId="3",
                    ReceiverId="1"
                }
            };

            await db.AddRangeAsync(messages);
            await db.SaveChangesAsync();

            var expected = new Message
            {
                Id = 1,
                Content = "Test",
                AuthorId = "1",
                ReceiverId = "3",
            };

            var messagesService = new MessageService(db, null);
            var message = await messagesService.GetLastMessageAsync("1", "3");

            message.Should().BeEquivalentTo(expected.Content);

        }

        [Fact]
        public async Task GetLastActivityAsyncShouldReturnLastestCreatedOn()
        {
            var guid = Guid.NewGuid().ToString();
            var options = DatabaseConfigOptions(guid);

            var db = new YourMoviesDbContext(options);

            var messagesService = new MessageService(db, null);
            await messagesService.CreateMessageAsync("Test", "1", "2");

            var message = new Message
            {
                Id=2,
                Content = "Test",
                AuthorId = "1",
                ReceiverId = "3",
                CreatedOn = "01.04.2022 1.43"
            };

            await db.Messages.AddAsync(message);
            await db.SaveChangesAsync();

            var lastestActivity=messagesService.GetLastActivityAsync("1", "3");

            lastestActivity.Result.Should().BeEquivalentTo("01.04.2022 1.43");
        }

        [Fact]
        public async Task GetAllUserMessagesAsyncShouldReturnAllMessagesFromUser()
        {
            var guid = Guid.NewGuid().ToString();
            var options = DatabaseConfigOptions(guid);

            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var messagesService = new MessageService(db, mapper);
            await messagesService.CreateMessageAsync("Hello", "1", "2");
            await messagesService.CreateMessageAsync("How are you", "1", "3");  

            var messagesToFind = new List<Message>
            {
                new Message
                {
                    Content = "Hello",
                    AuthorId="1",
                    ReceiverId="3",
                },
                new Message
                {
                    Content="How are you",
                    AuthorId="1",
                    ReceiverId="3"
                }
            };


            var user = new ApplicationUser
            {
                Id = "1",
                ReceivedMessages = messagesToFind
            };

           await db.Users.AddAsync(user);
           await db.SaveChangesAsync();

            var actualMessages = messagesService.GetAllUserMessagesAsync<Message>("1", "1");

            actualMessages.Result.Should().BeEquivalentTo(user.ReceivedMessages);
        }

        private static DbContextOptions<YourMoviesDbContext> DatabaseConfigOptions(string guid)
         => new DbContextOptionsBuilder<YourMoviesDbContext>()
                          .UseInMemoryDatabase(guid)
                          .Options;

        private static MapperConfiguration MappingConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Message, Message>();
            });
        }
    }
}






