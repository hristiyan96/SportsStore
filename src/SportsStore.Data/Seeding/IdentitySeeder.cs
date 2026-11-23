using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Core.Entities;

namespace SportsStore.Data.Seeding;

public static class IdentitySeeder
{
    public const string UserRole = "User";

    public const string AdministratorRole = "Administrator";

    public const string AdminEmail = "admin@sportsstore.local";

    public const string DevelopmentPassword = "Admin123!";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        var users = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { UserRole, AdministratorRole })
        {
            if (!await roles.RoleExistsAsync(role))
            {
                var created = await roles.CreateAsync(new IdentityRole(role));
                if (!created.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(" ", created.Errors.Select(error => error.Description)));
                }
            }
        }

        var admin = await users.FindByEmailAsync(AdminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                EmailConfirmed = true,
                FirstName = "Store",
                LastName = "Admin"
            };

            var created = await users.CreateAsync(admin, DevelopmentPassword);
            if (!created.Succeeded)
            {
                throw new InvalidOperationException(string.Join(" ", created.Errors.Select(error => error.Description)));
            }
        }

        if (!await users.IsInRoleAsync(admin, AdministratorRole))
        {
            await users.AddToRoleAsync(admin, AdministratorRole);
        }
    }
}
