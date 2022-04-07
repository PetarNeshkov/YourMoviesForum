using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using YourMovies.Web.Infrastructure;
using YourMoviesForum;
using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Data;
using YourMoviesForum.Services.Data.Categories;
using YourMoviesForum.Services.Data.Messages;
using YourMoviesForum.Services.Data.PostReactions;
using YourMoviesForum.Services.Data.Posts;
using YourMoviesForum.Services.Data.Replies;
using YourMoviesForum.Services.Data.ReplyReactions;
using YourMoviesForum.Services.Data.Tags;
using YourMoviesForum.Services.Data.Users;
using YourMoviesForum.Services.Providers.DateTime;
using YourMoviesForum.Services.Providers.Email;
using YourMoviesForum.Services.Providers.Security_Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(
           this IServiceCollection services,
           IConfiguration configuration)
           => services
               .AddDbContext<YourMoviesDbContext>(options => options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        { 
            services
                .AddDefaultIdentity<ApplicationUser>(options => options
                    .SetIdentityOptions())
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<YourMoviesDbContext>();

            return services;
        }


        public static IServiceCollection AddAntiforgeryHeader(this IServiceCollection services)
          => services
              .AddAntiforgery(options => options
                  .HeaderName = "X-CSRF-TOKEN");

        public static IServiceCollection AddControllersWithAutoAntiforgeryTokenAttribute(this IServiceCollection services)
        {    
            services
                    .AddControllersWithViews(options => options
                    .Filters.Add<AutoValidateAntiforgeryTokenAttribute>())
                    .AddRazorRuntimeCompilation();

            return services;
        }

        public static IServiceCollection AddFacebookAuthentication(
           this IServiceCollection services,
           IConfiguration configuration)
        {
           services
                 .AddAuthentication()
                 .AddFacebook(facebookOptions =>
                 {
                     facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                     facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                 });

            return services;
        }

        public static IServiceCollection AddGoogleAuthentication(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            services
                .AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                });

            return services;
        }


        public static IServiceCollection AddCookiePolicyOptions(this IServiceCollection services)
           => services
                .Configure<CookiePolicyOptions>(options => options
                    .CookiePolicyOptions());
            

        public static IServiceCollection AddApplicationServices(
           this IServiceCollection services,
           IConfiguration configuration)
        { 
           services
               .AddSingleton(configuration)
               .AddTransient<IEmailSender, SendGridEmailSender>()
                    .AddTransient<IPostService, PostService>()
                    .AddTransient<ICategoryService, CategoryService>()
                    .AddTransient<ITagService, TagService>()
                    .AddTransient<IDateTimeProvider, DateTimeProvider>()
                    .AddTransient<IUserService, User>()
                    .AddTransient<IReplyService, ReplyService>()
                    .AddTransient<IPostReactionService, PostReactionService>()
                    .AddTransient<IReplyReactionService, ReplyReactionService>()
                    .AddTransient<IMessageService, MessageService>()
                    .Configure<ReCaptchSettings>(configuration.GetSection("GoogleReCaptcha"))
                    .AddSignalR();

            return services;
        }
    }
}
