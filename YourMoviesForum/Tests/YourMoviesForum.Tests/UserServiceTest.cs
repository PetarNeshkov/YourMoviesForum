using AutoMapper;
using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

using Moq;
using Xunit;
using FluentAssertions;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Providers.DateTime;
using YourMoviesForum.Services.Data.Users;

namespace YourMoviesForum.Tests
{
    public class UserServiceTest
    {
        [Theory]
        [InlineData(3, 1)]
        [InlineData(2, 2)]
        public async Task AddRatingToUserAsyncShouldIncreaseUserPoints(int rating, int pointsToAdd)
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var user = new ApplicationUser
            {
                Id = guid,
                UserName = "User test",
                Email = "test@test.com",
                Rating = rating,
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var usersService = new User(db, null, dateTimeProvider.Object);
            await usersService.AddRatingToUserAsync(guid, pointsToAdd);

            var actual = await db.Users.FirstAsync();

            actual.Rating.Should().Be(4);
        }

        [Theory]
        [InlineData("User 1")]
        [InlineData("User 2")]
        [InlineData("User 3")]
        public async Task IsUsernameUsedAsyncShouldReturnTrueIfUsernameIsUsed(string username)
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var user = new ApplicationUser
            {
                Id = guid,
                UserName = username,
                Email = "test@abv.bg",
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var usersService = new User(db, null, dateTimeProvider.Object);
            var isUsernameUsed = await usersService.IsUsernameUsedAsync(username);

            isUsernameUsed.Should().BeTrue();
        }

        [Theory]
        [InlineData("User 1")]
        [InlineData("User 2")]
        [InlineData("User 3")]
        public async Task IsUsernameUsedAsyncShouldReturnFalseIfUsernameIsNotUsed(string username)
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var usersService = new User(db, null, dateTimeProvider.Object);
            var isUsernameUsed = await usersService.IsUsernameUsedAsync(username);

            isUsernameUsed.Should().BeFalse();
        }

        [Theory]
        [InlineData("userOne@abv.bg")]
        [InlineData("userTwo@abv.bg")]
        [InlineData("userThree@abv.bg")]
        public async Task IsEmailUsedAsyncShouldReturnTrueIfUsernameIsUsed(string email)
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var user = new ApplicationUser
            {
                Id = guid,
                UserName = "Test User",
                Email = email,
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var usersService = new User(db, null, dateTimeProvider.Object);
            var isEmailUsed = await usersService.IsEmailUsedAsync(email);

            isEmailUsed.Should().BeTrue();
        }

        [Theory]
        [InlineData("userOne@abv.bg")]
        [InlineData("userTwo@abv.bg")]
        [InlineData("userThree@abv.bg")]
        public async Task IsEmailUsedAsyncShouldReturnFalseIfEmailIsNotUsed(string email)
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var usersService = new User(db, null, dateTimeProvider.Object);
            var isEmailUsed = await usersService.IsUsernameUsedAsync(email);

            isEmailUsed.Should().BeFalse();
        }

        private static DbContextOptions<YourMoviesDbContext> DatabaseConfigOptions(string guid)
          => new DbContextOptionsBuilder<YourMoviesDbContext>()
                           .UseInMemoryDatabase(guid)
                           .Options;
    }
}
