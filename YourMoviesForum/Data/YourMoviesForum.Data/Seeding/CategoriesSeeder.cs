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
                new Category { Name = "Sci-Fi", CreatedOn=DateTime.UtcNow },
                new Category { Name = "Action", CreatedOn=DateTime.UtcNow  },
                new Category { Name = "Horror", CreatedOn=DateTime.UtcNow  },
                new Category { Name = "Romantic", CreatedOn=DateTime.UtcNow  },
                new Category { Name = "Cartoon", CreatedOn=DateTime.UtcNow  },
                new Category { Name = "Adventure", CreatedOn=DateTime.UtcNow  },
                new Category { Name = "Actors", CreatedOn=DateTime.UtcNow  },
                new Category { Name = "Tragedy", CreatedOn=DateTime.UtcNow  }
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
