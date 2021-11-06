using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

using YourMoviesForum.Data.Models;

namespace YourMoviesForum.Data.Seeding
{
    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(YourMoviesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Categories.AnyAsync())
            {
                return;
            }

           await dbContext.Categories.AddRangeAsync(new[]
            {
                new Category { Name = "Sci-Fi" },
                new Category { Name = "Action" },
                new Category { Name = "Horror" },
                new Category { Name = "Romantic" },
                new Category { Name = "Cartoon" },
                new Category { Name = "Adventure" }
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
