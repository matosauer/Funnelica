using Application.Queries;
using Application.Services;
using Asp.Versioning.Conventions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using Persistence.Queries;
using Persistence.Repositories;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IGetProductByIdQuery, GetProductByIdQuery>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Funnelika") ?? throw new InvalidOperationException("Connection string not found!")));

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    //options.ReportApiVersions = true;
                })
                .AddMvc(options =>
                {
                    options.Conventions.Add(new VersionByNamespaceConvention());
                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
