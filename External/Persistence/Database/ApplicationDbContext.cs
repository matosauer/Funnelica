using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products => Set<Product>();
}
