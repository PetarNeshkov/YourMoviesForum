using AutoMapper;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Moq;
using Xunit;
using FluentAssertions;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Providers.DateTime;
using YourMoviesForum.Services.Data.Posts;
using YourMoviesForum.Services.Data.Users;

namespace YourMoviesForum.Tests
{
    public class PostServiceTest
    {
        [Theory]
        [InlineData("Star Wars", "Blockbuster!", 1)]
        [InlineData("Ben 10","Best anime ever", 2)]
        [InlineData("Generator Rex", "It was awsome one!", 3)]
        public async Task CreatePostAsyncShouldAddPostInDatabase(string title, string content, int categoryId)
        {
            var guid = Guid.NewGuid().ToString();
            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var usersServiceMock = new Mock<IUserService>();
            dateTimeProvider.Setup(dtp => dtp.Now())
                    .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            for (int i = 0; i < 5; i++)
            {
               await db.Tags.AddAsync(new Tag
                {
                    Name = $"Tag: {i}",
                });
            }
            await db.SaveChangesAsync();

            var expectedIds = new[] { 1, 2, 3 };

            var postsService = new PostService(db, null, dateTimeProvider.Object, usersServiceMock.Object);
            var postId = await postsService.CreatePostAsync(title,content, categoryId, expectedIds,guid);

            var actual = await db.Posts.FirstAsync();
            var actualTagIds = actual.Tags.Select(t => t.Id).ToArray();

            postId.Should().Be(1);
            db.Posts.Should().HaveCount(1);
            actualTagIds.Should().BeEquivalentTo(expectedIds);
        }


        [Fact]
        public async Task GetPostsSearchCountAsyncShouldReturnProperCount()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var category = new Category
            {
                Name = "Success"
            };

            var posts = new List<Post>
            {
                new Post { Title = "Star Wars",Content="Best movie ever!",Category=category},
                new Post { Title = "Paul Walker",Content="One of the best actors",Category=category},
                new Post { Title = "Star Wars The Clone Wars",Content="Very good anime!",Category=category}
            };

            await db.Posts.AddRangeAsync(posts);
            await db.SaveChangesAsync();

            var postService = new PostService(db, null, null,null);
            var actualPosts = await postService.GetPostsSearchCountAsync("Star Wars");

            Assert.Equal(2, actualPosts);
        }

        [Fact]
        public async Task GetPostsSearchCountAsyncShouldReturnProperCountForCategories()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var firstCategory = new Category
            {
                Name = "Success"
            };

            var secondCategory = new Category
            {
                Name = "Fail"
            };

            var thirdCategory = new Category
            {
                Name = "BlockBuster"
            };
            var posts = new List<Post>
            {
                new Post { Title = "Star Wars",Content="Best movie ever!",Category=firstCategory},
                new Post { Title = "Paul Walker",Content="One of the best actors",Category=secondCategory},
                new Post { Title = "Star Wars The Clone Wars",Content="Very good anime!",Category=thirdCategory}
            };

            await db.Posts.AddRangeAsync(posts);
            await db.SaveChangesAsync();

            var postService = new PostService(db, null, null, null);
            var actualPosts = await postService.GetPostsSearchCountAsync("Success");

            Assert.Equal(1, actualPosts);
        }

        [Theory]
        [InlineData("Star Wars", "Blockbuster!", 1)]
        [InlineData("Ben 10", "Best anime ever", 2)]
        [InlineData("Generator Rex", "It was awsome one!", 3)]
        public async Task EditPostAsyncShouldWorkCorrectly(string title, string content, int categoryId)
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now())
                   .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            var post = new Post
            {
                Id = 1,
                Title = "Fast and Furious",
                Content = "I am very dissapointed",
                CategoryId = 1,
                AuthorId = guid,
                Tags = new List<Tag>
                {
                    new Tag {Id=1, Name="Wow" },
                    new Tag {Id=2, Name="Great"},
                    new Tag {Id=3, Name="Supeer"}
                }
            };

            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();

            var postsService = new PostService(db, null, dateTimeProvider.Object,null);
            await postsService.EditPostAsync(1, title, content, categoryId, new[] {1});
            var actual = await db.Posts.FirstOrDefaultAsync();

            var expected = new Post
            {
                Id = 1,
                Title = title,
                Content = content,
                CreatedOn = dateTimeProvider.Object.Now(),
                ModifiedOn = dateTimeProvider.Object.Now(),
                CategoryId = categoryId,
                AuthorId = guid,
                Tags = new List<Tag>
                {
                   new Tag { Id=1, Name = "Wow", CreatedOn = dateTimeProvider.Object.Now(),
                   Posts=new List<Post>{post} } ,
                }
            };

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Star Wars", "Blockbuster!", 1)]
        [InlineData("Ben 10", "Best anime ever", 2)]
        [InlineData("Generator Rex", "It was awsome one!", 3)]
        public async Task DeletePostAsyncShouldSetDeletedOnAndIsDeletedToTrue(string title, string content, int categoryId)
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now())
                   .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            var post = new Post
            {
                Id = 1,
                Title = title,
                Content = content,
                CategoryId = categoryId,
                AuthorId = guid,
                Tags = new List<Tag>
                {
                    new Tag {Id=1, Name="Wow" },
                    new Tag {Id=2, Name="Great"},
                    new Tag {Id=3, Name="Supeer"}
                }
            };

            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();

            var postsService = new PostService(db, null, dateTimeProvider.Object, null);

            await postsService.DeletePostAsync(1);

            var actual = await db.Posts.FirstAsync();

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().BeSameAs(dateTimeProvider.Object.Now());
        }

        [Theory]
        [InlineData("Star Wars", "Blockbuster!", 1)]
        [InlineData("Ben 10", "Best anime ever", 2)]
        [InlineData("Generator Rex", "It was awsome one!", 3)]
        public async Task ViewMethodIncreasePostViewsByOne(string title, string content, int categoryId)
        {
            var guid = Guid.NewGuid().ToString();
            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now())
                  .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            var post = new Post
            {
                Id = 1,
                Title = title,
                Content = content,
                CategoryId = categoryId,
                AuthorId = guid,
                CreatedOn = dateTimeProvider.Object.Now(),
                Tags = new List<Tag>
                {
                    new Tag {Id=1, Name="Wow" }
                }
            };

            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();

            var postsService = new PostService(db, null, dateTimeProvider.Object,null);
            await postsService.ViewAsync(1);

            var actual = await db.Posts.FirstAsync();
            actual.Rating.Should().Be(1);
        }

        [Fact]
        public async Task GetPostAuthorIdAsyncShouldReturnPostAuthorId()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var post = new Post
            {
                Title = "Yugioh",
                Content = "It's a very nice one!",
                AuthorId = "Test author id",
            };

            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();

            var postsService = new PostService(db, null, null,null);

            var authorId = await postsService.GetPostAuthorIdAsync<Post>(1);

            authorId.Should().BeSameAs(post.AuthorId);
        }

        [Fact]
        public async Task GetPostAuthorIdAsyncShouldReturnNullIfPostIsDeleted()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var post = new Post
            {
                Title = "Super class",
                Content = "It's my lifee",
                CategoryId = 1,
                AuthorId = "Test author id",
                IsDeleted = true,
                CreatedOn = dateTimeProvider.Object.Now(),
                DeletedOn = dateTimeProvider.Object.Now()
            };

            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();

            var postsService = new PostService(db, null, null,null);

            var authorId = await postsService.GetPostAuthorIdAsync<Post>(1);

            authorId.Should().BeNull();
        }

        [Fact]
        public async Task GetAllPostByIdAsyncShouldReturnNullWhenPostIsDeleted()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            await db.Posts.AddAsync(new Post
            {
                Title = "Super",
                Content = "Greaaat!",
                IsDeleted = true
            });
            await db.SaveChangesAsync();

            var postService = new PostService(db, mapper, dateTimeProvider.Object,null);
            var actual = await postService.GetByIdAsync<Post>(1);

            actual.Should().BeNull();
        }


        [Fact]
        public async Task GetAllPostByIdAsyncShouldReturnNullWhenPostIsNotFound()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var postService = new PostService(db, mapper, dateTimeProvider.Object,null);
            var actual = await postService.GetByIdAsync<Post>(1);

            actual.Should().BeNull();
        }


        [Fact]
        public async Task GetLatestPostActivityAsyncShouldReturnPostLatestActivity()
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now())
                  .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            var post = new Post
            {
                Title = "Latest news in movies branch!",
                Content = "Listen up what I have to tell!",
                CategoryId = 1,
                AuthorId = guid
            };

            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();

            var postsService = new PostService(db, null, dateTimeProvider.Object,null);

            var latestActivity = await postsService.GetLatestPostActivityAsync(1);

            latestActivity.Should().Be("0min");
        }


        [Fact]
        public async Task GetLatestPostActivityAsyncShouldReturnPostLatestActivityWithReply()
        {
            var guid = Guid.NewGuid().ToString();

            var options = DatabaseConfigOptions(guid);
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now())
                 .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            var post = new Post
            {
                Title = "Latest news in movies branch!",
                Content = "Listen up what I have to tell!",
                CategoryId = 1,
                AuthorId = guid
            };

            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();


            var postsService = new PostService(db, null, dateTimeProvider.Object, null);
            var postToFind = await db.Posts.FirstAsync(x => x.Id == 1);

            var reply = new Reply
            {
                Id = 1,
                Content = "Nice one!"
            };
            await db.Replies.AddAsync(reply);
            postToFind.Replies=new List<Reply>() {reply};

            await db.SaveChangesAsync();

            var latestActivity = await postsService.GetLatestPostActivityAsync(1);

            latestActivity.Should().Be("0min");
        }

            private static MapperConfiguration MappingConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Post, Post>();
            });
        }

        private static DbContextOptions<YourMoviesDbContext> DatabaseConfigOptions(string guid)
          => new DbContextOptionsBuilder<YourMoviesDbContext>()
                           .UseInMemoryDatabase(guid)
                           .Options;
    }
}
