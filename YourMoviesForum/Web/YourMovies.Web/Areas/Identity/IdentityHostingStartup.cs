using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YourMovies.Web.Areas.Identity.Data;

[assembly: HostingStartup(typeof(YourMovies.Web.Areas.Identity.IdentityHostingStartup))]
namespace YourMovies.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            //builder.ConfigureServices((context, services) => {
            //    services.AddDbContext<YourMoviesDbContext>(options =>
            //        options.UseSqlServer(
            //            context.Configuration.GetConnectionString("YourMoviesDbContextConnection")));

            //    services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //        .AddEntityFrameworkStores<YourMoviesDbContext>();
            //});

            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}