using System;

using System.Threading.Tasks;

namespace YourMoviesForum.Data.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(YourMoviesDbContext dbContext, IServiceProvider serviceProvider);
    }
}
