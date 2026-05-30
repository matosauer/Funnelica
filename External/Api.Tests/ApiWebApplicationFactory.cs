using Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Persistence.Database;

namespace Api.Tests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IProductService> ProductServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Funnelica"] = "Server=(localdb)\\mssqllocaldb;Database=FunnelicaTests;Trusted_Connection=True;"
            });
        });

        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (dbContextDescriptor is not null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase($"FunnelicaTests_{Guid.NewGuid()}"));

            var productServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IProductService));
            if (productServiceDescriptor is not null)
            {
                services.Remove(productServiceDescriptor);
            }

            services.AddScoped(_ => ProductServiceMock.Object);
        });
    }
}
