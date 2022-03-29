using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;

using YourMoviesForum;
using YourMovies.Web.Infrastructure;
using YourMoviesForum.Services.Data;
using YourMoviesForum.Services.Data.Posts;
using YourMoviesForum.Services.Data.Categories;
using YourMoviesForum.Services.Data.Tags;
using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Data.Users;
using YourMoviesForum.Services.Providers.DateTime;
using YourMoviesForum.Services.Providers.Email;
using YourMoviesForum.Services.Providers.Security_Models;
using YourMoviesForum.Services.Data.Replies;
using YourMoviesForum.Services.Data.PostReactions;
using YourMoviesForum.Services.Data.ReplyReactions;
using YourMoviesForum.Services.Data.Messages;
using YourMovies.Web.Chat;

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

            services.AddAntiforgery(o => o.HeaderName = "X-CSRF-TOKEN");
            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }).AddRazorRuntimeCompilation();


            services.AddAuthentication()
              .AddFacebook(facebookOptions =>
              {
                  facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                  facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
              })
              .AddGoogle(googleOptions =>
              {
                  googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                  googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
              });

            //Application services
            services.AddTransient<IEmailSender, SendGridEmailSender>()
                    .AddTransient<IPostService, PostService>()
                    .AddTransient<ICategoryService, CategoryService>()
                    .AddTransient<ITagService, TagService>()
                    .AddTransient<IDateTimeProvider, DateTimeProvider>()
                    .AddTransient<IUserService, User>()
                    .AddTransient<IReplyService, ReplyService>()
                    .AddTransient<IPostReactionService,PostReactionService>()
                    .AddTransient<IReplyReactionService,ReplyReactionService>()
                    .AddTransient<IMessageService,MessageService>();
                    

            services.AddSignalR();

            services.Configure<ReCaptchSettings>(this.configuration.GetSection("GoogleReCaptcha"));

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
                .UseResponseCaching()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();

                    endpoints.MapHub<ChatHub>("/chat");
                });
        }
    }
}
