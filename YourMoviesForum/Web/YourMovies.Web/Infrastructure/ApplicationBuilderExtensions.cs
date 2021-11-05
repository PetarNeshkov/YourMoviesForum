using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourMoviesForum;
using YourMoviesForum.Data.Models;

namespace YourMovies.Web.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        //checks whether there are new migrations and apply them when the program starts
        public static  IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
           //Creating a scope where the code will exist
           using var scopedServices = app.ApplicationServices.CreateScope();

           var data=scopedServices.ServiceProvider.GetService<YourMoviesDbContext>();

            data.Database.Migrate();

            SeedCategories(data);

            return app;
        }

        public  static void SeedCategories(YourMoviesDbContext data)
        {
            if (data.Posts.Any())
            {
                return;
            }

            var categories = new List<Category>
            {
                new Category
                {
                    Name="Sci-Fi"
                },
                 new Category
                {
                    Name="Action"
                },
                  new Category
                {
                    Name="Horror"
                },
                   new Category
                {
                    Name="Romantic"
                },
                    new Category
                {
                    Name="Cartoon"
                },
                     new Category
                {
                    Name="Sci-Fi"
                }
            };


            data.Categories.AddRange(categories);
        }
    }
}
