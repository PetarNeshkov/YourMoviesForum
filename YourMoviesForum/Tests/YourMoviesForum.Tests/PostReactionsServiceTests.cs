using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Xunit;

using FluentAssertions;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Data.PostReactions;
using YourMoviesForum.Web.InputModels.Reactions.enums;
using YourMoviesForum.Web.InputModels.Reactions;

using Moq;

namespace YourMoviesForum.Tests
{
    public class PostReactionsServiceTests
    {

        [Theory]
        [InlineData("Comedy", "Best one yet!", ReactionType.Like)]
        public async Task ReactAsyncShouldAddReaction(string title, string content, ReactionType type)
        {
            var guid=new Guid().ToString();

            var options = DatabaseConfigOptions(guid);

            var db = new YourMoviesDbContext(options);

            var post = new Post
            {
                Id = 1,
                Title = title,
                Content = content,
                CategoryId = 1,
                AuthorId = guid,
            };

            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();

            var postReactionsService = new PostReactionService(db);
            var result = await postReactionsService.ReactAsync(type, 1, guid);

            var actual = await db.PostReactions.FirstOrDefaultAsync();

            var expected = new PostReaction
            {
                Id = 1,
                PostId = 1,
                Post = post,
                AuthorId = guid,
                ReactionType = type,
                CreatedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm")
            };

            actual.Should().BeEquivalentTo(expected);
            result.Should().BeOfType<ReactionCountServiceModel>();
        }

        [Theory]
        [InlineData(ReactionType.Heart)]
        [InlineData(ReactionType.Haha)]
        [InlineData(ReactionType.Wow)]
        [InlineData(ReactionType.Sad)]
        [InlineData(ReactionType.Angry)]
        public async Task ReactAsyncShouldChangeReaction(ReactionType type)
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);

            var db = new YourMoviesDbContext(options);

            var postReaction = new PostReaction
            {
                Id = 1,
                PostId = 1,
                AuthorId = guid,
                ReactionType = ReactionType.Like,
            };

            await db.PostReactions.AddAsync(postReaction);
            await db.SaveChangesAsync();

            var postReactionsService = new PostReactionService(db);
            var result = await postReactionsService.ReactAsync(type, 1, guid);

            var actual = await db.PostReactions.FirstOrDefaultAsync();
            var expected = new PostReaction
            {
                Id = 1,
                PostId = 1,
                AuthorId = guid,
                ReactionType = type,
                CreatedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"),
                ModifiedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm")
            };

            actual.Should().BeEquivalentTo(expected);
            result.Should().BeOfType<ReactionCountServiceModel>();
        }

        [Theory]
        [InlineData(ReactionType.Heart)]
        [InlineData(ReactionType.Haha)]
        [InlineData(ReactionType.Wow)]
        [InlineData(ReactionType.Sad)]
        [InlineData(ReactionType.Angry)]
        public async Task ReactMethodShouldChangeReactionIfAlreadyExistsAndChangeModifiedOn(ReactionType type)
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);

            var db = new YourMoviesDbContext(options);

            var postReaction = new PostReaction
            {
                Id = 1,
                PostId = 1,
                AuthorId = guid,
                ReactionType = ReactionType.Like,
                CreatedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm")
            };

            await db.PostReactions.AddAsync(postReaction);
            await db.SaveChangesAsync();

            var postReactionsService = new PostReactionService(db);
            var result = await postReactionsService.ReactAsync(type, 1, guid);

            var actual = await db.PostReactions.FirstOrDefaultAsync();
            var expected = new PostReaction
            {
                Id = 1,
                PostId = 1,
                AuthorId = guid,
                ReactionType = type,
                CreatedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"),
                ModifiedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm")
            };

            actual.Should().BeEquivalentTo(expected);
            result.Should().BeOfType<ReactionCountServiceModel>();
        }

        private static DbContextOptions<YourMoviesDbContext> DatabaseConfigOptions(string guid)
        {
            return new DbContextOptionsBuilder<YourMoviesDbContext>()
                            .UseInMemoryDatabase(guid)
                            .Options;
        }
    }
}