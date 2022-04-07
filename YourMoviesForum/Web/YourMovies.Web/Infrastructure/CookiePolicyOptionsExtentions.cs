using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace YourMovies.Web.Infrastructure
{

    public static class CookiePolicyOptionsExtentions
    {
        public static CookiePolicyOptions CookiePolicyOptions(this CookiePolicyOptions options)
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;

            return options;
        }
    }
}
