using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourMoviesForum.Data.Seeding
{
    public class YourMoviesDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(YourMoviesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext==null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var seeders = new List<ISeeder>
            {
                new AdministratorSeeder(),
                new CategoriesSeeder(),
                new TagsSeeder(),
                new PostsSeeder()
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
