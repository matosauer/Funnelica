using Microsoft.AspNetCore.Identity;
using Persistence.DependencyInjection;

namespace Web.Services
{
    public static class WebServiceExtensions
    {
        public static IServiceCollection AddFunnelicaWebIdentity(this IServiceCollection services)
        {
            services.AddFunnelicaIdentityCore();

            services.AddAuthentication(IdentityConstants.ApplicationScheme)
                    .AddIdentityCookies();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            return services;
        }
    }
}
