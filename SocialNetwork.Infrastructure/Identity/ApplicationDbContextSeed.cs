using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Elomoas.Infrastructure.Identity;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Create roles if they don't exist
        var roles = new[] { "User", "Manager", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create default admin user
        var adminEmail = "admin@elomoas.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Create default manager users
        var managerUsers = new[]
        {
            ("manager1@elomoas.com", "Manager1!"),
            ("manager2@elomoas.com", "Manager2!")
        };

        foreach (var (email, password) in managerUsers)
        {
            var managerUser = await userManager.FindByEmailAsync(email);
            
            if (managerUser == null)
            {
                managerUser = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };
                
                var result = await userManager.CreateAsync(managerUser, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(managerUser, "Manager");
                }
            }
        }

        // Get all users and their roles in a single query
        var allUsers = await userManager.Users.ToListAsync();
        var userRoles = new Dictionary<string, List<string>>();
        
        foreach (var user in allUsers)
        {
            userRoles[user.Id] = (await userManager.GetRolesAsync(user)).ToList();
        }

        // Assign User role to users who don't have any role
        foreach (var user in allUsers)
        {
            if (!userRoles[user.Id].Any())
            {
                await userManager.AddToRoleAsync(user, "User");
            }
        }
    }
} 