using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using YourMoviesForum.Data.Models;

namespace YourMoviesForum.Data.Seeding
{
    public class TagsSeeder : ISeeder
    {
        public async Task SeedAsync(YourMoviesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Tags.AnyAsync())
            {
                return;
            }

            await dbContext.Tags.AddRangeAsync(new[]
            {
                new Tag{Name="Acting"},
                new Tag{Name="Actors"},
                new Tag{Name="IMDb"},
                new Tag{Name="Perfomance"},
                new Tag{Name="Cinema"},
                new Tag{Name="Blockbuster"},
                new Tag{Name="3D"},
                new Tag{Name="4D"},
                new Tag{Name="Success"},
                new Tag{Name="Fail"},
                new Tag{Name="Trailer"},
                new Tag{Name="Upcoming"}
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
