using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Persistence.Identity;

public class IdentitySeeder
{
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IConfiguration configuration;
    private readonly ILogger<IdentitySeeder> logger;

    public IdentitySeeder(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        ILogger<IdentitySeeder> logger)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
        this.configuration = configuration;
        this.logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedRolesAsync(cancellationToken);
        await SeedAdminUserAsync(cancellationToken);
    }

    private async Task SeedRolesAsync(CancellationToken cancellationToken)
    {
        foreach (var roleName in IdentityRoles.All)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }

            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                logger.LogInformation("Created identity role {RoleName}.", roleName);
            }
            else
            {
                logger.LogError(
                    "Failed to create identity role {RoleName}: {Errors}",
                    roleName,
                    string.Join(", ", result.Errors.Select(error => error.Description)));
            }
        }
    }

    private async Task SeedAdminUserAsync(CancellationToken cancellationToken)
    {
        var seedSection = configuration.GetSection("IdentitySeed");
        var email = seedSection["AdminEmail"];
        var password = seedSection["AdminPassword"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogDebug("IdentitySeed admin credentials are not configured; skipping admin user seeding.");
            return;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is not null)
        {
            return;
        }

        var adminUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FullName = seedSection["AdminFullName"]
        };

        var createResult = await userManager.CreateAsync(adminUser, password);
        if (!createResult.Succeeded)
        {
            logger.LogError(
                "Failed to create seed admin user {Email}: {Errors}",
                email,
                string.Join(", ", createResult.Errors.Select(error => error.Description)));
            return;
        }

        var roleResult = await userManager.AddToRoleAsync(adminUser, IdentityRoles.Administrator);
        if (roleResult.Succeeded)
        {
            logger.LogInformation("Created seed admin user {Email}.", email);
        }
        else
        {
            logger.LogError(
                "Created seed admin user {Email} but failed to assign role {RoleName}: {Errors}",
                email,
                IdentityRoles.Administrator,
                string.Join(", ", roleResult.Errors.Select(error => error.Description)));
        }
    }
}
