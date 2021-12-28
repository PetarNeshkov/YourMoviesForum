using Microsoft.AspNetCore.Hosting;

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