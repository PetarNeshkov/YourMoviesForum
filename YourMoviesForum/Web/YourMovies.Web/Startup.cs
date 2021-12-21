using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using YourMoviesForum;
using YourMovies.Web.Infrastructure;
using YourMoviesForum.Services.Data;
using YourMoviesForum.Services.Data.Posts;
using YourMoviesForum.Services.Data.Categories;
using YourMoviesForum.Services.Data.Tags;
using YourMoviesForum.Data.Models;
using Microsoft.AspNetCore.Mvc;
using YourMoviesForum.Services.Providers.DateTime;
using ForumNet.Services.Providers.DateTime;
using YourMoviesForum.Services.Data.Users;

namespace YourMovies.Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<YourMoviesDbContext>(options => options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<YourMoviesDbContext>();

            services.AddControllersWithViews();

            services.AddAutoMapper(typeof(MappingProfiler));


            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }).AddRazorRuntimeCompilation();

            //Application services
            services.AddTransient<IPostService, PostService>()
                    .AddTransient<ICategoryService, CategoryService>()
                    .AddTransient<ITagService, TagService>()
                    .AddTransient<IDateTimeProvider,DateTimeProvider>()
                    .AddTransient<IUserService,User>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app
                   .UseDeveloperExceptionPage()
                   .UseMigrationsEndPoint();
            }
            else
            {
                app
                    .UseExceptionHandler("/Home/Error")
                    .UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                });
        }
    }
}
