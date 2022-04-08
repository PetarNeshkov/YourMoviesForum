using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Threading.Tasks;

using YourMoviesForum.Data.Models;
using YourMoviesForum.Services.Providers.Background;
using static YourMoviesForum.Common.GlobalConstants.Administrator;

namespace YourMoviesForum.Data.Seeding
{
    public class AdministratorSeeder : ISeeder
    {
        public async Task SeedAsync(YourMoviesDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var isExistingAdmin = await roleManager.RoleExistsAsync(AdministratorRoleName);

            if (!isExistingAdmin)
            {

                var role = new ApplicationRole { Name= AdministratorRoleName };

                await roleManager.CreateAsync(role);

                var admin = new ApplicationUser
                {
                    UserName = AdministratorUsername,
                    Email = AdministratorEmail,
                    EmailConfirmed = true,
                    BackgroundColor = BackgroundProvider.BackgroundPicker()
                };

                admin.FirstLetter = char.ToUpper(admin.UserName[0]);

                await userManager.CreateAsync(admin, AdministratorPassword);

                await userManager.AddToRoleAsync(admin, role.Name);
            }
        }
    }
}
