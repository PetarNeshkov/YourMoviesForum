using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System.Linq;

using YourMoviesForum;
using YourMoviesForum.Data.Models;

namespace YourMovies.Web.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        //checks whether there are new migrations and apply them when the program starts
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
           //Creating a scope where the code will exist
           using var scopedServices = app.ApplicationServices.CreateScope();

           var data=scopedServices.ServiceProvider.GetService<YourMoviesDbContext>();

            data.Database.Migrate();

            app.ApplicationServices.GetService<YourMoviesDbContext>();

            return app;
        }

        public static void SeedPosts(YourMoviesDbContext data)
        {
            if (data.Posts.Any())
            {
                return;
            }

            data.Categories.AddRange(new[]
                {
                    new Category {Name="Sci-Fi"},
                    new Category {Name="Action"},
                    new Category {Name="Horror"},
                    new Category {Name="Sci-Fi"},
                    new Category {Name="Romantic"},
                    new Category {Name="Cartoon"}
                });
        }
    }
}
