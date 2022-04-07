using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                .AddDatabase(configuration)
                .AddDatabaseDeveloperPageExceptionFilter()
                .AddIdentity()
                .AddCookiePolicyOptions()
                .AddAntiforgeryHeader()
                .AddControllersWithAutoAntiforgeryTokenAttribute()
                .AddAutoMapper(typeof(MappingProfiler))
                .AddFacebookAuthentication(configuration)
                .AddGoogleAuthentication(configuration)
                .AddApplicationServices(configuration)
                .AddControllersWithViews();

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
                .UseCookiePolicy()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();

                    endpoints.MapHub<ChatHub>("/chat");
                });
        }
    }
}
