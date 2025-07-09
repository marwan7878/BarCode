using Infrastructure.Enums;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Seeders
{
    public static class DefaultUsers
    {
     
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed the DB (Users table) with 1 Admin user
            var defaultUser = new ApplicationUser
            {
                FirstName = "admin",
                LastName = "admin",
                UserName = "admin@test.com",
                Email = "admin@test.com",
                EmailConfirmed = true,
            };

            // Check if the user doesn't exist in BD => Add it 
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user is null)
            {
                await userManager.CreateAsync(defaultUser, "Marwan123#");
                //add the user to all roles (becasue the super admin has all the permissions)
                await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                await userManager.AddToRoleAsync(defaultUser, Roles.Doctor.ToString());
                await userManager.AddToRoleAsync(defaultUser, Roles.Patient.ToString());
            }

        }
    }
}
