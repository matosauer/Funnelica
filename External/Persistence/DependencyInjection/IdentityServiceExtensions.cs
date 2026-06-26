using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database;
using Persistence.Identity;

namespace Persistence.DependencyInjection;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddFunnelicaIdentityCore(this IServiceCollection services)
    {
        services.AddDataProtection();

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddScoped<IdentitySeeder>();

        return services;
    }
}
