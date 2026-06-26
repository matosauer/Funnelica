using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Identity;

namespace Persistence.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
}
