using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Data.Categories;
using YourMoviesForum.Services.Providers.DateTime;

namespace YourMoviesForum.Tests
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task GetAllCategoriesAsyncShouldReturnAllCategoriesAndCorrectType()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
          
            var expectedCategories = new List<Category>();

            for (int i = 0; i < 5; i++)
            {
                expectedCategories.Add(new Category
                {
                    Name = $"Category: {i}",
                    
                });
            }

            await db.Categories.AddRangeAsync(expectedCategories);
            await db.SaveChangesAsync();

            var categoriesService = new CategoryService(db, mapper, dateTimeProvider.Object);
            var actualCategories = await categoriesService.GetAllCategoriesAsync<Category>();

            actualCategories.Should().BeEquivalentTo(expectedCategories).And.AllBeOfType<Category>();
        }

        [Fact]
        public async Task GetAllMethodShouldNotReturnDeletedCategoriesAndShouldReturnZero()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var expectedCategories = new List<Category>();

            for (int i = 0; i < 5; i++)
            {
                expectedCategories.Add(new Category
                {
                    Name = $"Category: {i}",
                    IsDeleted = true,
                });
            }

            await db.Categories.AddRangeAsync(expectedCategories);
            await db.SaveChangesAsync();

            var categoriesService = new CategoryService(db, mapper, dateTimeProvider.Object);
            var actual = await categoriesService.GetAllCategoriesAsync<Category>();

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllMethodShouldZeroCategories()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var categoriesService = new CategoryService(db, mapper, dateTimeProvider.Object);
            var actual = await categoriesService.GetAllCategoriesAsync<Category>();

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetSearchedCategories()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now())
                .Returns(DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm"));
            
            var categories = new List<Category>
            {
                new Category { Name = "Category"},
                new Category { Name = "Test "},
                new Category { Name = "Best Category"}
            };

            await db.Categories.AddRangeAsync(categories);
            await db.SaveChangesAsync();

            var expectedCategories = new List<Category>
            {
                new Category { Id = 1, Name = "Category",CreatedOn=dateTimeProvider.Object.Now()},
                new Category { Id = 3, Name = "Best Category",CreatedOn=dateTimeProvider.Object.Now()}
            };
            
            var categoriesService = new CategoryService(db, mapper, dateTimeProvider.Object);
            var actualCategories = await categoriesService.GetAllCategoriesAsync<Category>("Category",0,6);

            actualCategories.Should().BeEquivalentTo(expectedCategories).And.AllBeOfType<Category>();
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

            var categories = new List<Category>
            {
                new Category { Name = "Category"},
                new Category { Name = "Test "},
                new Category { Name = "Best Category"}
            };

            await db.Categories.AddRangeAsync(categories);
            await db.SaveChangesAsync();

            var categoriesService = new CategoryService(db, mapper, dateTimeProvider.Object);
            var actualCategories = await categoriesService.GetPostsSearchCountAsync("Test");

            Assert.Equal(1, actualCategories);
        }

        [Fact]
        public async Task GetCategoryByIdAsyncShouldReturnCorrectCategory()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            await db.Categories.AddAsync(new Category
            {
                Name = "Performance",
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoryService(db, mapper, dateTimeProvider.Object);
            var expected = await categoriesService.GetCategoryByIdAsync<Category>(1);
            var actual = await db.Categories.FirstAsync();

            actual.Id.Should().Be(expected.Id);
            actual.Name.Should().BeSameAs(expected.Name);         
        }

        [Fact]
        public async Task GetCategoryByIdMethodShouldReturnModelWhichIsNotDeleted()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            await db.Categories.AddAsync(new Category
            {
                Name = "Performance",
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoryService(db, mapper, dateTimeProvider.Object);
            var expected = await categoriesService.GetCategoryByIdAsync<Category>(1);
            var actual = await db.Categories.FirstAsync();

            actual.IsDeleted.Should().Be(expected.IsDeleted);
        }

        [Fact]
        public async Task GetCategoryByIdMethodShouldReturnNullWhenCategoryIsDeleted()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var config = MappingConfiguration();
            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            await db.Categories.AddAsync(new Category
            {
                Name = "Performance",
                IsDeleted = true
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoryService(db, mapper, dateTimeProvider.Object);
            var actual = await categoriesService.GetCategoryByIdAsync<Category>(1);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetCategoryByIdMethodShouldReturnNullWhenCategoryIsNotFound()
        {
            var options=DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db=new YourMoviesDbContext(options);

            var config=MappingConfiguration();
            var mapper=config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var categoriesService = new CategoryService(db, mapper, dateTimeProvider.Object);
            var actual = await categoriesService.GetCategoryByIdAsync<Category>(1);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task IsExistingAsyncWithIdParameterShouldReturnTrueIfExists()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            await db.Categories.AddAsync(new Category
            {
                Name = "Performance",
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoryService(db, null, dateTimeProvider.Object);
            var isExisting = await categoriesService.IsExistingAsync(1);

            isExisting.Should().BeTrue();
        }

        [Theory]
        [InlineData("Performance")]
        [InlineData("Success")]
        [InlineData("Fail")]
        public async Task IsExistingAsyncWithNameParameterShouldReturnTrueIfExists(string name)
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            await db.Categories.AddAsync(new Category
            {
                Name = name,
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoryService(db, null, dateTimeProvider.Object);
            var isExisting = await categoriesService.IsExistingAsync(name);

            isExisting.Should().BeTrue();
        }

        [Theory]
        [InlineData("Performance")]
        [InlineData("Success")]
        [InlineData("Fail")]
        public async Task IsExistingAsyncWithNameParameterShouldReturnFalseIfNotExists(string name)
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var categoriesService = new CategoryService(db, null, dateTimeProvider.Object);
            var isExisting = await categoriesService.IsExistingAsync(name);

            isExisting.Should().BeFalse();
        }

        [Fact]
        public async Task IsExistingAsyncWithIdParameterShouldReturnFalseIfNotExists()
        {
            var options = DatabaseConfigOptions(Guid.NewGuid().ToString());
            var db = new YourMoviesDbContext(options);

            var dateTimeProvider = new Mock<IDateTimeProvider>();

            var categoriesService = new CategoryService(db, null, dateTimeProvider.Object);
            var isExisting = await categoriesService.IsExistingAsync(1);

            isExisting.Should().BeFalse();
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

            var categoriesService = new CategoryService(db, mapper, dateTimeProvider.Object);
            await categoriesService.CreateAsync("Performance");

            var expected = new Category
            {
                Id = 1,
                Name = "Performance",
                CreatedOn = dateTimeProvider.Object.Now()     
            };

            var actual = await db.Categories.FirstOrDefaultAsync();

            db.Categories.Should().HaveCount(1);
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

            await db.Categories.AddAsync(new Category
            {
                Name = "Performance",
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoryService(db, null, dateTimeProvider.Object);
            await categoriesService.DeleteAsync(1);

            var actual = await db.Categories.FirstAsync();

            actual.IsDeleted.Should().BeTrue();
            actual.DeletedOn.Should().BeSameAs(dateTimeProvider.Object.Now());
        }


        private static MapperConfiguration MappingConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, Category>();
            });
        }


        private static DbContextOptions<YourMoviesDbContext> DatabaseConfigOptions(string guid)
          => new DbContextOptionsBuilder<YourMoviesDbContext>()
                           .UseInMemoryDatabase(guid)
                           .Options;
    }
}
