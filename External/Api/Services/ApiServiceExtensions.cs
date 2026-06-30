using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Persistence.DependencyInjection;
using System.Text;

namespace Api.Services
{
    public static class ApiServiceExtensions
    {
        public static IServiceCollection AddFunnelicaApiAuth(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddFunnelicaIdentityCore();

            var jwt = configuration.GetSection("Jwt");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt["Issuer"],
                        ValidAudience = jwt["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwt["Key"]!))
                    };
                });
            return services;
        }
    }
}
