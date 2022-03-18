using AutoMapper;
using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Moq;
using Xunit;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Providers.DateTime;
using YourMoviesForum.Services.Data.Tags;
using FluentAssertions;
using System.Linq;

namespace YourMoviesForum.Tests
{
    public class TagServiceTest
    {
        [Fact]
        public async Task GetAllTagsAsyncShouldReturnAllTagsAndCorrectType()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var expectedTags = new List<Tag>();

            for (int i = 0; i < 5; i++)
            {
                expectedTags.Add(new Tag
                {
                    Name = $"Tag: {i}",

                });
            }

            await db.Tags.AddRangeAsync(expectedTags);
            await db.SaveChangesAsync();

            var categoriesService = new TagService(db, mapper, dateTimeProvider.Object);
            var actualCategories = await categoriesService.GetAllTagsAsync<Tag>();

            actualCategories.Should().BeEquivalentTo(expectedTags).And.AllBeOfType<Tag>();

        }

        [Fact]
        public async Task GetAllTagsAsyncShouldNotReturnDeletedCategoriesAndShouldReturnZero()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var expectedTags = new List<Tag>();

            for (int i = 0; i < 5; i++)
            {
                expectedTags.Add(new Tag
                {
                    Name = $"Tag: {i}",
                    IsDeleted = true,
                });
            }

            await db.Tags.AddRangeAsync(expectedTags);
            await db.SaveChangesAsync();

            var categoriesService = new TagService(db, mapper, dateTimeProvider.Object);
            var actual = await categoriesService.GetAllTagsAsync<Tag>();

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllTagsAsyncShouldReturnZeroTags()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var categoriesService = new TagService(db, mapper, dateTimeProvider.Object);
            var actual = await categoriesService.GetAllTagsAsync<Tag>();

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetSearchedTags()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now())
                .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            var tags = new List<Tag>
            {
                new Tag { Name = "Tag"},
                new Tag { Name = "Best"},
                new Tag { Name = "Best Tag"}
            };

            await db.Tags.AddRangeAsync(tags);
            await db.SaveChangesAsync();

            var expectedTags = new List<Tag>
            {
                new Tag { Id = 1, Name = "Tag",CreatedOn=dateTimeProvider.Object.Now()},
                new Tag { Id = 3, Name = "Best Tag",CreatedOn=dateTimeProvider.Object.Now()}
            };

            var categoriesService = new TagService(db, mapper, dateTimeProvider.Object);
            var actualCategories = await categoriesService.GetAllTagsAsync<Tag>("Tag", 0, 6);

            actualCategories.Should().BeEquivalentTo(expectedTags).And.AllBeOfType<Tag>();
        }

        [Fact]
        public async Task GetPostsSearchCountAsyncShouldReturnProperCount()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now())
                    .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            var tags = new List<Tag>
            {
                new Tag { Name = "Tag"},
                new Tag { Name = "Test "},
                new Tag { Name = "Best Tag"}
            };

            await db.Tags.AddRangeAsync(tags);
            await db.SaveChangesAsync();

            var tagService = new TagService(db, mapper, dateTimeProvider.Object);
            var actualTags = await tagService.GetPostsSearchCountAsync("Test");

            Assert.Equal(1, actualTags);
        }

        [Fact]
        public async Task GetTagByIdAsyncShouldReturnModelWhichIsNotDeleted()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            await db.Tags.AddAsync(new Tag
            {
                Name = "Great"
            });
            await db.SaveChangesAsync();

            var categoriesService = new TagService(db, mapper, dateTimeProvider.Object);
            var expected = await categoriesService.GetTagByIdAsync<Tag>(1);
            var actual = await db.Tags.FirstAsync();

            actual.IsDeleted.Should().Be(expected.IsDeleted);
        }

        [Fact]
        public async Task GetTagByIdMethodShouldReturnNullWhenCategoryIsDeleted()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            await db.Tags.AddAsync(new Tag
            {
                Name = "Super",
                IsDeleted = true
            });
            await db.SaveChangesAsync();

            var tagService = new TagService(db, mapper, dateTimeProvider.Object);
            var actual = await tagService.GetTagByIdAsync<Tag>(1);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetTagByIdMethodShouldReturnNullWhenCategoryIsNotFound()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var tagService = new TagService(db, mapper, dateTimeProvider.Object);
            var actual = await tagService.GetTagByIdAsync<Tag>(1);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task IsExistingAsyncWithIdParameterShouldReturnTrueIfExists()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            await db.Tags.AddAsync(new Tag
            {
                Name = "Poor",
            });
            await db.SaveChangesAsync();

            var tagService = new TagService(db, null, dateTimeProvider.Object);
            var isExisting = await tagService.IsExistingAsync(1);

            isExisting.Should().BeTrue();
        }

        [Theory]
        [InlineData("Fail")]
        [InlineData("Success")]
        [InlineData("IMDb")]
        public async Task IsExistingAsyncWithNameParameterShouldReturnTrueIfExists(string name)
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            await db.Tags.AddAsync(new Tag
            {
                Name = name,
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var tagService = new TagService(db, null, dateTimeProvider.Object);
            var isExisting = await tagService.IsExistingAsync(name);

            isExisting.Should().BeTrue();
        }

        [Fact]
        public async Task IsExistingAsyncWithIdParameterShouldReturnFalseIfNotExists()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var tagService = new TagService(db, null, dateTimeProvider.Object);
            var isExisting = await tagService.IsExistingAsync(1);

            isExisting.Should().BeFalse();
        }

        [Theory]
        [InlineData("Fail")]
        [InlineData("Success")]
        [InlineData("IMDb")]
        public async Task IsExistingAsyncWithNameParameterShouldReturnFalseIfNotExists(string name)
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var categoriesService = new TagService(db, null, dateTimeProvider.Object);
            var isExisting = await categoriesService.IsExistingAsync(name);

            isExisting.Should().BeFalse();
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetAllByPostIdAsyncShouldWorkCorrectAndReturnCorrectType(int postId)
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            for (int i = 1; i <= 3; i++)
            {
                await db.Tags.AddAsync(new Tag
                {
                    Name = $"Tag {i}",
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    Posts = new List<Post>
                    { 
                        new Post 
                        { 
                            Id = i ,
                            Title="Best best best",
                            Content ="Best post"
                        } 
                    }
                });
            }
            await db.SaveChangesAsync();

            var tagsService = new TagService(db, mapper, dateTimeProvider.Object);
            var actual = await tagsService.GetAllPostsByIdAsync<Tag>(postId);
            var expected = await db.Posts
                .Where(pt => pt.Id == postId)
                .SelectMany(pt => pt.Tags)
                .Where(t => !t.IsDeleted)
                .ToListAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetAllByPostIdAsyncShouldReturnZeroItemsIfThereAreNotAnyTagsWithThisPostId(int postId)
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            for (int i = 1; i <= 3; i++)
            {
                await db.Tags.AddAsync(new Tag
                {
                    Name = $"Tag {i}",
                    IsDeleted = true,
                    Posts = new List<Post> 
                    {
                        new Post 
                        { 
                            Id = i,
                            Title="Best one!",
                            Content="Best content"
                        }
                    }
                });
            }
            await db.SaveChangesAsync();

            var tagsService = new TagService(db, mapper, dateTimeProvider.Object);
            var actual = await tagsService.GetAllPostsByIdAsync<Tag>(postId);

            actual.Should().BeEmpty();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetAllByPostIdAsyncShouldReturnZeroItems(int postId)
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());

            var db = new YourMoviesDbContext(options);

            var config=MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            for (int i = 1; i <= 3; i++)
            {
                await db.Tags.AddAsync(new Tag
                {
                    Name = $"Test {i}",
                    IsDeleted = true,
                    CreatedOn = dateTimeProvider.Object.Now(),
                    Posts = new List<Post> 
                    { 
                        new Post 
                        { 
                            Id = i ,
                            Title="It's a test bro",
                            Content="Amazing one!"
                        } 
                    }
                });
            }
            await db.SaveChangesAsync();

            var tagsService = new TagService(db, mapper, dateTimeProvider.Object);
            var actual = await tagsService.GetAllPostsByIdAsync<Tag>(postId);

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateAsyncShouldAddCategory()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now())
                    .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            var tagsService = new TagService(db, mapper, dateTimeProvider.Object);
            await tagsService.CreateAsync("Performance");

            var expected = new Category
            {
                Id = 1,
                Name = "Performance",
                CreatedOn = dateTimeProvider.Object.Now()
            };

            var actual = await db.Tags.FirstOrDefaultAsync();

            db.Tags.Should().HaveCount(1);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task DeleteAsyncShouldChangeIsDeletedAndDeletedOn()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now())
                    .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));

            await db.Tags.AddAsync(new Tag
            {
                Name = "Performance",
            });
            await db.SaveChangesAsync();

            var tagsService = new TagService(db, null, dateTimeProvider.Object);
            await tagsService.DeleteAsync(1);

            var actual = await db.Tags.FirstAsync();

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().BeSameAs(dateTimeProvider.Object.Now());
        }

        private static MapperConfiguration MappingConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tag, Tag>();
            });
        }


        private static DbContextOptions<YourMoviesDbContext> DatabaseConfigOptions(string guid)
          => new DbContextOptionsBuilder<YourMoviesDbContext>()
                           .UseInMemoryDatabase(guid)
                           .Options;
    }
}
