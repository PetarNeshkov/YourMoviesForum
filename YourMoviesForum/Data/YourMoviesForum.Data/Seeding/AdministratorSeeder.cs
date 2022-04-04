using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using YourMoviesForum.Data.Models;

using static YourMoviesForum.Common.GlobalConstants.Administrator;

namespace YourMoviesForum.Data.Seeding
{
    public class AdministratorSeeder : ISeeder
    {
        private List<string> BackgroundColors =
            new List<string> { "3C79B2", "FF8F88", "6FB9FF", "C0CC44", "AFB28C", "#8B0000", "#808080", "#FFFACD", "#66CDAA", "#800000", "#4169E1", "#2E8B57" };
        public async Task SeedAsync(YourMoviesDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var isExistingAdmin = await roleManager.RoleExistsAsync(AdministratorRoleName);

            if (!isExistingAdmin)
            {

                var role = new ApplicationRole { Name= AdministratorRoleName };

                await roleManager.CreateAsync(role);

                var randomIndex = new Random().Next(0, BackgroundColors.Count - 1);
                var bgColor = BackgroundColors[randomIndex];

                var admin = new ApplicationUser
                {
                    UserName = AdministratorUsername,
                    Email = AdministratorEmail,
                    EmailConfirmed = true,
                    BackgroundColor = bgColor
                };

                admin.FirstLetter = char.ToUpper(admin.UserName[0]);

                await userManager.CreateAsync(admin, AdministratorPassword);

                await userManager.AddToRoleAsync(admin, role.Name);
            }
        }
    }
}
