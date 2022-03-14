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

            var createdOn = DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm");

           await dbContext.Categories.AddRangeAsync(new[]
            {
                new Category { Name = "Sci-Fi", CreatedOn=createdOn},
                new Category { Name = "Action", CreatedOn=createdOn},
                new Category { Name = "Horror", CreatedOn=createdOn},
                new Category { Name = "Romantic", CreatedOn=createdOn},
                new Category { Name = "Cartoon", CreatedOn=createdOn},
                new Category { Name = "Adventure", CreatedOn=createdOn},
                new Category { Name = "Actors", CreatedOn=createdOn},
                new Category { Name = "Tragedy", CreatedOn=createdOn}
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
