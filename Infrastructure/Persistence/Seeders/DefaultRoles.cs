using Infrastructure.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Seeders
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Doctor.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Patient.ToString()));
            }
        }
    }
}
