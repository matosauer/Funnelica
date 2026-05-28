using Application.Services;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using Persistence.Repositories;
using Scalar.AspNetCore;
using Serilog;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfwork>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Funnelica") ?? throw new InvalidOperationException("Connection string not found!")));

            builder.Services.AddControllers();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddApiVersioning()
            .AddApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddOpenApi("v1");
            builder.Services.AddOpenApi("v2");

            builder.Host.UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(
                    context.Configuration
                );
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.AddDocuments("v1", "v2");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
