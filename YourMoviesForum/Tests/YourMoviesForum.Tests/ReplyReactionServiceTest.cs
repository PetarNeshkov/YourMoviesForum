using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

using Xunit;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Data.ReplyReactions;
using YourMoviesForum.Web.InputModels.Reactions;
using YourMoviesForum.Web.InputModels.Reactions.enums;

namespace YourMoviesForum.Tests
{
    public class ReplyReactionServiceTest
    {
        [Theory]
        [InlineData("Best one yet!", ReactionType.Like)]
        public async Task ReactAsyncShouldAddReaction(string content, ReactionType type)
        {
            var guid= Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var reply = new Reply
            {
                Id = 1,
                Content = content,
                AuthorId = guid,
                CreatedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm")
            };

            await db.Replies.AddAsync(reply);
            await db.SaveChangesAsync();

            var replyReactionService = new ReplyReactionService(db);
            var result= await replyReactionService.ReactAsync(type,1,guid);

            var actual = await db.ReplyReactions.FirstOrDefaultAsync();
            var expected = new ReplyReaction
            {
                Id=1,
                ReplyId=1,
                Reply=reply,
                AuthorId=guid,
                ReactionType=type,
                CreatedOn= DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm")
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

            var options= DatabaseConfigOptions(guid);

            var db=new YourMoviesDbContext(options);

            var replyReacton = new ReplyReaction
            {
                Id = 1,
                ReplyId = 1,
                AuthorId=guid,
                ReactionType= ReactionType.Like,
                CreatedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"),
                ModifiedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm")
            };

            await db.ReplyReactions.AddAsync(replyReacton);
            await db.SaveChangesAsync();

            var replyReactionService=new ReplyReactionService(db);
            var result=await replyReactionService.ReactAsync(type,1,guid);

            var actual = await db.ReplyReactions.FirstOrDefaultAsync();
            var expected = new ReplyReaction
            {
                Id=1,
                ReplyId=1,
                AuthorId= guid,
                ReactionType = type,
                CreatedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"),
                ModifiedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm")
            };

            actual.Should().BeEquivalentTo(expected);
            result.Should().BeOfType<ReactionCountServiceModel>();
        }

        [Theory]
        [InlineData(ReactionType.Like)]
        [InlineData(ReactionType.Heart)]
        [InlineData(ReactionType.Haha)]
        [InlineData(ReactionType.Wow)]
        [InlineData(ReactionType.Sad)]
        [InlineData(ReactionType.Angry)]
        public async Task ReactMethodShouldChangeReactionToNone(ReactionType type)
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var replyReaction = new ReplyReaction
            {
                Id = 1,
                ReplyId = 1,
                AuthorId = guid,
                ReactionType = type,
                CreatedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm")
            };

            await db.ReplyReactions.AddAsync(replyReaction);
            await db.SaveChangesAsync();

            var replyReactionsService = new ReplyReactionService(db);
            var result = await replyReactionsService.ReactAsync(type, 1, guid);

            var actual = await db.ReplyReactions.FirstOrDefaultAsync();
            var expected = new ReplyReaction
            {
                Id = 1,
                ReplyId = 1,
                AuthorId = guid,
                ReactionType = ReactionType.None,
                CreatedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"),
                ModifiedOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm")
            };

            actual.Should().BeEquivalentTo(expected);
            result.Should().BeOfType<ReactionCountServiceModel>();
        }

        private static DbContextOptions<YourMoviesDbContext> DatabaseConfigOptions(string guid)
           => new DbContextOptionsBuilder<YourMoviesDbContext>()
                            .UseInMemoryDatabase(guid)
                            .Options;

    }
}
