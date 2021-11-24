using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using YourMoviesForum;
using YourMoviesForum.Data.Seeding;

namespace YourMovies.Web.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        //checks whether there are new migrations and apply them when the program starts
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            //Creating a scope where the code will exist
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<YourMoviesDbContext>();

            data.Database.Migrate();

            SeedData(app);

            return app;
        }

        public static IApplicationBuilder SeedData(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            using var dbContext = scopedServices.ServiceProvider.GetService<YourMoviesDbContext>();

            new YourMoviesDbContextSeeder()
                 .SeedAsync(dbContext, scopedServices.ServiceProvider)
                 .GetAwaiter()
                 .GetResult();

            return app;
        }
    }
}
