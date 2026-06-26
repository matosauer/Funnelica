using Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Persistence.Database;

namespace Api.Tests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IProductService> ProductServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var dbContextDescriptors = services
                .Where(d =>
                    d.ServiceType == typeof(ApplicationDbContext) ||
                    d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>))
                .ToList();

            foreach (var descriptor in dbContextDescriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase($"FunnelicaTests_{Guid.NewGuid()}"));

            // Remove the existing IProductService registration
            var productServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IProductService));
            if (productServiceDescriptor is not null)
            {
                services.Remove(productServiceDescriptor);
            }

            // Add the mocked IProductService
            services.AddScoped(_ => ProductServiceMock.Object);
        });
    }
}
