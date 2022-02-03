using System.Security.Claims;

using static YourMoviesForum.Common.GlobalConstants.Administrator;

namespace YourMovies.Web.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static bool IsAdministrator(this ClaimsPrincipal claimsPrincipal)
           => claimsPrincipal.IsInRole(AdministratorRoleName);
    }
}
