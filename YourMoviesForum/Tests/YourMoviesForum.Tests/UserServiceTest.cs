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
using System.Collections.Generic;
using System.Linq;

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


        [Theory]
        [InlineData("user1", "user1@test.com")]
        [InlineData("user2", "user2@test.com")]
        [InlineData("user3", "user3@test.com")]
        public async Task GetUserByIdAsyncShouldReturnCorrectModel(string userName, string email)
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var expected = new ApplicationUser
            {
                Id = guid,
                UserName = userName,
                Email = email
            };

            await db.Users.AddAsync(expected);
            await db.SaveChangesAsync();

            var usersService = new User(db, mapper, dateTimeProvider.Object);
            var actual = await usersService.GetUserByIdAsync<ApplicationUser>(guid);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("user1", "user1@test.com")]
        [InlineData("user2", "user2@test.com")]
        [InlineData("user3", "user3@test.com")]
        public async Task GetUserByIdAsyncShouldReturnNullIfUserIsDeleted(string userName, string email)
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var expected = new ApplicationUser
            {
                Id = guid,
                UserName = userName,
                Email = email,
                IsDeleted = true,
                CreatedOn = dateTimeProvider.Object.Now(),
                DeletedOn = dateTimeProvider.Object.Now()
            };

            await db.Users.AddAsync(expected);
            await db.SaveChangesAsync();

            var usersService = new User(db, mapper, dateTimeProvider.Object);
            var actual = await usersService.GetUserByIdAsync<ApplicationUser>(guid);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetAllUsersAsyncShouldReturnCorrectModels()
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var expected = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "user 1",
                    Email = "user1@test.com",
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "user 2",
                    Email = "user2@test.com",
                }
            };

            await db.Users.AddRangeAsync(expected);
            await db.SaveChangesAsync();

            var usersService = new User(db, mapper, dateTimeProvider.Object);
            var actual = await usersService.GetAllUsersAsync<ApplicationUser>();

            actual.Should().BeEquivalentTo(expected);
            actual.Should().HaveCount(expected.Count);
        }

        [Fact]
        public async Task GetAllUsersAsyncShouldNotReturnDeletedUsers()
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "user 1",
                    Email = "user1@test.com",
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    DeletedOn = dateTimeProvider.Object.Now()
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "user 2",
                    Email = "user2@test.com",
                    CreatedOn = dateTimeProvider.Object.Now()
                }
            };

            await db.Users.AddRangeAsync(users);
            await db.SaveChangesAsync();

            var usersService = new User(db, mapper, dateTimeProvider.Object);
            var actual = await usersService.GetAllUsersAsync<ApplicationUser>();

            actual.Should().HaveCount(1);
            actual.First().Should().BeEquivalentTo(users[1]);
        }

        private static DbContextOptions<YourMoviesDbContext> DatabaseConfigOptions(string guid)
          => new DbContextOptionsBuilder<YourMoviesDbContext>()
                           .UseInMemoryDatabase(guid)
                           .Options;

        private static MapperConfiguration MappingConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, ApplicationUser>();
            });
        }
    }
}
