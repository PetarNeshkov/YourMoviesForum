using System;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;


using Xunit;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Data.Replies;
using YourMoviesForum.Services.Data.Users;
using YourMoviesForum.Services.Providers.DateTime;

namespace YourMoviesForum.Tests
{
    public class ReplyServiceTest
    {
        [Theory]
        [InlineData("Wow!", 1, 1)]
        [InlineData("Nice one!", null, 1)]
        public async Task CreateMethodShouldAddReplyInDatabase(string content, int? parentId, int postId)
        {
            var guid = Guid.NewGuid().ToString();
            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var usersServiceMock = new Mock<IUserService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var repliesService = new ReplyService(db, usersServiceMock.Object, null, dateTimeProviderMock.Object);
            await repliesService.CreateReplyAsync(content, parentId, postId, guid);

            db.Replies.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("Wow", 1, 1)]
        [InlineData("Nice one!", null, 1)]
        public async Task CreateMethodShouldAddProperEntities(string content, int? parentId, int postId)
        {
            var guid = Guid.NewGuid().ToString();
            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var usersServiceMock = new Mock<IUserService>();
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now())
                    .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            var repliesService = new ReplyService(db, usersServiceMock.Object, null, dateTimeProvider.Object);
            await repliesService.CreateReplyAsync(content, parentId, postId, guid);

            var expected = new Reply
            {
                Id = 1,
                Content = content,
                ParentId = parentId,
                PostId = postId,
                AuthorId = guid,
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var actual = await db.Replies.FirstAsync();

            expected.Id.Should().Be(actual.Id);
            expected.Content.Should().Be(actual.Content);
            expected.ParentId.Should().Be(actual.ParentId);
            expected.PostId.Should().Be(actual.PostId);
            expected.AuthorId.Should().Be(actual.AuthorId);
            expected.CreatedOn.Should().Be(actual.CreatedOn);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnNullWhenReplyIsDeleted()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var usersServiceMock = new Mock<IUserService>();

            await db.Replies.AddAsync(new Reply
            {
                Content = "I am not happy!",
                IsDeleted = true
            });
            await db.SaveChangesAsync();

            var replyService = new ReplyService(db, usersServiceMock.Object, mapper, dateTimeProvider.Object);
            var actual = await replyService.GetByIdAsync<Reply>(1);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnNullWhenReplyIsNotFound()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var usersServiceMock = new Mock<IUserService>();

            var replyService = new ReplyService(db, usersServiceMock.Object, mapper, dateTimeProvider.Object);
            var actual = await replyService.GetByIdAsync<Reply>(1);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetReplyAuthorIdAsyncShouldReturnCorrectId()
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var usersServiceMock = new Mock<IUserService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            await db.Replies.AddAsync(new Reply
            {
                Content = "I really like this one!",
                AuthorId = guid,
            });
            await db.SaveChangesAsync();

            var repliesService = new ReplyService(db,usersServiceMock.Object, null,dateTimeProviderMock.Object);
            var authorId = await repliesService.GetReplyAuthorIdAsync<Reply>(1);

            authorId.Should().BeSameAs(guid);
        }

        [Fact]
        public async Task GetReplyAuthorIdAsyncShouldReturnNullIfReplyIsNotFound()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var usersServiceMock = new Mock<IUserService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var repliesService = new ReplyService(db,usersServiceMock.Object,null,dateTimeProviderMock.Object);
            var authorId = await repliesService.GetReplyAuthorIdAsync<Reply>(1);

            authorId.Should().BeNull();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetAllRepliesByPostIdAsyncShouldWorkCorrectAndReturnCorrectType(int postId)
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var usersServiceMock = new Mock<IUserService>();
            var dateTimeProvider = new Mock<IDateTimeProvider>();

            for (int i = 1; i <= 3; i++)
            {
                await db.Replies.AddAsync(new Reply
                {
                    Content = "Nicee",
                    PostId=i
                });
            }
            await db.SaveChangesAsync();

            var replyService = new ReplyService(db, usersServiceMock.Object, mapper, dateTimeProvider.Object);
            var actual = await replyService.GetAllRepliesByPostIdAsync<Reply>(postId);
            var expected = await db.Posts
                .Where(pt => pt.Id == postId)
                .SelectMany(pt => pt.Replies)
                .Where(t => !t.IsDeleted)
                .ToListAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetAllRepliesByPostIdAsyncShouldReturnZeroItemsIfThereAreNotAnyRepliesWithThisPostId(int postId)
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var usersServiceMock = new Mock<IUserService>();
            var dateTimeProvider = new Mock<IDateTimeProvider>();

            for (int i = 1; i <= 3; i++)
            {
                await db.Replies.AddAsync(new Reply
                {
                    Content = $"It is very bad post!",
                    IsDeleted = true,
                });
            }
            await db.SaveChangesAsync();

            var replyService = new ReplyService(db,usersServiceMock.Object, mapper, dateTimeProvider.Object);
            var actual = await replyService.GetAllRepliesByPostIdAsync<Reply>(postId);

            actual.Should().BeEmpty();
        }

        [Theory]
        [InlineData("Nice one!", "Editing it!")]
        [InlineData("Blabla", "Editing it!")]
        [InlineData("Oops", "Editing it!")]
        public async Task EditMethodShouldChangeDescriptionAndModifiedOn(string createdContent, string editedContent)
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var usersServiceMock = new Mock<IUserService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(dtp => dtp.Now())
                     .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            await db.Replies.AddAsync(new Reply
            {
                Content = createdContent,
            });
            await db.SaveChangesAsync();

            var replyService = new ReplyService(db, usersServiceMock.Object, null , dateTimeProviderMock.Object);
            await replyService.EditAsync(1, editedContent);

            var expected = new Reply
            {
                Content = editedContent,
                ModifiedOn = dateTimeProviderMock.Object.Now()
            };

            var actual = await db.Replies.FirstAsync();

            expected.Content.Should().BeSameAs(actual.Content);
            expected.ModifiedOn.Should().Be(actual.ModifiedOn);
        }

        [Theory]
        [InlineData("Nice one!")]
        [InlineData("Blabla")]
        [InlineData("Oops")]
        public async Task DeleteAsyncShouldChangeIsDeletedAndApplyDeletedOn(string content)
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var usersServiceMock = new Mock<IUserService>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(dtp => dtp.Now())
                    .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            await db.Replies.AddAsync(new Reply
            {
                Content = content,
            });
            await db.SaveChangesAsync();

            var repliesService = new ReplyService(db, usersServiceMock.Object,null,dateTimeProviderMock.Object);
            await repliesService.DeleteAsync(1);

            var expected = new Reply
            {
                Id = 1,
                IsDeleted = true,
                DeletedOn = dateTimeProviderMock.Object.Now()
            };

            var actual = await db.Replies.FirstAsync();

            expected.Id.Should().Be(actual.Id);
            expected.IsDeleted.Should().Be(actual.IsDeleted);
            expected.DeletedOn.Should().Be(actual.DeletedOn);
        }

        private static MapperConfiguration MappingConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reply, Reply>();
            });
        }

        private static DbContextOptions<YourMoviesDbContext> DatabaseConfigOptions(string guid)
          => new DbContextOptionsBuilder<YourMoviesDbContext>()
                           .UseInMemoryDatabase(guid)
                           .Options;
    }

}

